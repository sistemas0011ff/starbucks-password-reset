
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;
using Microsoft.Extensions.Logging;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.repositories;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.value_objects;
using SR.PasswordReset.Api.src.contexts.shared.configuration;
using System.Security.Cryptography;
using System.Text;

namespace SR.PasswordReset.Api.src.contexts.password_reset.infrastructure.repositories
{
    public class CognitoRepository : ICognitoRepository
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoClient;
        private readonly AWSConfiguration _awsConfig;
        private readonly ILogger<CognitoRepository> _logger;
        private readonly int _otpExpirationMinutes;

        public CognitoRepository(
            IAmazonCognitoIdentityProvider cognitoClient,
            AWSConfiguration awsConfig,
            ILogger<CognitoRepository> logger)
        {
            _cognitoClient = cognitoClient;
            _awsConfig = awsConfig;
            _logger = logger;
            _otpExpirationMinutes = int.Parse(Environment.GetEnvironmentVariable("OTP_EXPIRATION_MINUTES") ?? "3");

        }

        public async Task ConfirmUserExists(Email email)
        {
            _logger.LogInformation("Verificando usuario en Cognito: {Email}", email.Value);

            try
            {
                var getUserRequest = new AdminGetUserRequest
                {
                    UserPoolId = _awsConfig.UserPoolId,
                    Username = email.Value
                };

                var userResponse = await _cognitoClient.AdminGetUserAsync(getUserRequest);
                _logger.LogInformation("Usuario encontrado con status: {Status}", userResponse.UserStatus);
            }
            catch (UserNotFoundException)
            {
                _logger.LogError("Usuario no encontrado en Cognito: {Email}", email.Value);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar usuario en Cognito: {Email}", email.Value);
                throw;
            }
        }

        public async Task<bool> ValidateOtp(Email email, string code)
        {
            _logger.LogInformation("Validando código OTP para usuario: {Email}", email.Value);

            try
            {
                var secretHash = CalculateSecretHash(
                    email.Value,
                    _awsConfig.ClientId,
                    _awsConfig.ClientSecret
                );

                var confirmForgotPasswordRequest = new ConfirmForgotPasswordRequest
                {
                    ClientId = _awsConfig.ClientId,
                    Username = email.Value,
                    ConfirmationCode = code,
                    SecretHash = secretHash,
                    Password = "Temporal123!" // Esta contraseña será cambiada inmediatamente
                };

                _logger.LogInformation("Enviando solicitud de validación OTP");
                await _cognitoClient.ConfirmForgotPasswordAsync(confirmForgotPasswordRequest);

                _logger.LogInformation("Código OTP validado exitosamente");
                return true;
            }
            catch (CodeMismatchException)
            {
                _logger.LogWarning("Código OTP inválido para usuario: {Email}", email.Value);
                throw;
            }
            catch (ExpiredCodeException)
            {
                _logger.LogWarning("Código OTP expirado para usuario: {Email}", email.Value);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar código OTP: {Email}", email.Value);
                throw;
            }
        }

        public async Task SendOtp(Email email)
        {
            _logger.LogInformation("Iniciando envío de OTP para usuario: {Email}", email.Value);

            try
            {
                // Verificar usuario y su estado
                var getUserRequest = new AdminGetUserRequest
                {
                    UserPoolId = _awsConfig.UserPoolId,
                    Username = email.Value
                };

                var userResponse = await _cognitoClient.AdminGetUserAsync(getUserRequest);

                // Verificar si necesita cambio de contraseña forzado
                if (userResponse.UserStatus == UserStatusType.FORCE_CHANGE_PASSWORD)
                {
                    _logger.LogInformation("Usuario requiere cambio de contraseña, actualizando estado...");
                    var setPasswordRequest = new AdminSetUserPasswordRequest
                    {
                        UserPoolId = _awsConfig.UserPoolId,
                        Username = email.Value
                    };
                    await _cognitoClient.AdminSetUserPasswordAsync(setPasswordRequest);
                }

                // Generar y enviar OTP
                var secretHash = CalculateSecretHash(
                    email.Value,
                    _awsConfig.ClientId,
                    _awsConfig.ClientSecret
                );

                var forgotPasswordRequest = new ForgotPasswordRequest
                {
                    ClientId = _awsConfig.ClientId,
                    Username = email.Value,
                    SecretHash = secretHash,
                };

                _logger.LogInformation($"Enviando solicitud de OTP con tiempo de expiración de {_otpExpirationMinutes} minutos");
                await _cognitoClient.ForgotPasswordAsync(forgotPasswordRequest);
                _logger.LogInformation("OTP enviado exitosamente");

                _logger.LogInformation("Enviando solicitud de OTP");
                await _cognitoClient.ForgotPasswordAsync(forgotPasswordRequest);
                _logger.LogInformation("OTP enviado exitosamente");
            }
            catch (UserNotFoundException)
            {
                _logger.LogError("Usuario no encontrado en Cognito: {Email}", email.Value);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar OTP: {Email}", email.Value);
                throw;
            }
        }

        public async Task ResetPassword(Email email, string newPassword)
        {
            _logger.LogInformation("Iniciando reset de contraseña para: {Email}", email.Value);

            try
            {
                // Verificar usuario
                var getUserRequest = new AdminGetUserRequest
                {
                    UserPoolId = _awsConfig.UserPoolId,
                    Username = email.Value
                };

                await _cognitoClient.AdminGetUserAsync(getUserRequest);

                // Cambiar contraseña
                var setPasswordRequest = new AdminSetUserPasswordRequest
                {
                    UserPoolId = _awsConfig.UserPoolId,
                    Username = email.Value,
                    Password = newPassword,
                    Permanent = true
                };

                _logger.LogInformation("Actualizando contraseña");
                await _cognitoClient.AdminSetUserPasswordAsync(setPasswordRequest);
                _logger.LogInformation("Contraseña actualizada exitosamente");
            }
            catch (UserNotFoundException)
            {
                _logger.LogError("Usuario no encontrado: {Email}", email.Value);
                throw;
            }
            catch (InvalidPasswordException)
            {
                _logger.LogError("Contraseña inválida para: {Email}", email.Value);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al resetear contraseña: {Email}", email.Value);
                throw;
            }
        }
        private string CalculateSecretHash(string username, string clientId, string clientSecret)
        {
            var message = Encoding.UTF8.GetBytes(username + clientId);
            var key = Encoding.UTF8.GetBytes(clientSecret);

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(message);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
