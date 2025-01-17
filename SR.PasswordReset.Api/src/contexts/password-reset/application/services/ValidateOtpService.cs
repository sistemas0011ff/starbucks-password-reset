

using Amazon.CognitoIdentityProvider.Model;
using SR.PasswordReset.Api.src.contexts.password_reset.application.dtos;
using SR.PasswordReset.Api.src.contexts.password_reset.application.interfaces.services;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.exceptions;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.repositories;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.value_objects;
using UserNotFoundException = SR.PasswordReset.Api.src.contexts.password_reset.domain.exceptions.UserNotFoundException;

namespace SR.PasswordReset.Api.src.contexts.password_reset.application.services
{
    public class ValidateOtpService : IValidateOtpService
    {
        private readonly ICognitoRepository _cognitoRepository;
        private readonly IUserRepository _userRepository;

        public ValidateOtpService(
            ICognitoRepository cognitoRepository,
            IUserRepository userRepository)
        {
            _cognitoRepository = cognitoRepository;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse> Execute(ValidateOtpApplicationDto request)
        {
            try
            {
                // 1. Validar y crear los objetos de valor
                var email = Email.Create(request.Email);
                var code = Code.Create(request.Code);

                // 2. Verificar que el usuario existe en nuestra base de datos
                var userExists = await _userRepository.ExistsById(email);
                if (!userExists)
                {
                    return new ServiceResponse(
                        isSuccess: false,
                        code: "USER_NOT_FOUND",
                        userMessage: "El usuario no está registrado en el sistema",
                        detailMessage: $"No se encontró el usuario con email {email.Value}"
                    );
                }

                // 3. Verificar que el usuario existe en Cognito
                await _cognitoRepository.ConfirmUserExists(email);

                // 4. Validar el código OTP
                var isValid = await _cognitoRepository.ValidateOtp(email, code.Value);
                if (!isValid)
                {
                    return new ServiceResponse(
                        isSuccess: false,
                        code: "INVALID_OTP",
                        userMessage: "El código ingresado no es válido",
                        detailMessage: $"El código OTP para {email.Value} no es válido"
                    );
                }

                return new ServiceResponse(
                    isSuccess: true,
                    code: "VALIDATE_OTP_SUCCESS",
                    userMessage: "Código validado correctamente. Puede proceder a establecer su nueva contraseña.",
                    detailMessage: $"Código OTP validado exitosamente para el usuario {email.Value}"
                );
            }
            catch (ExpiredCodeException)
            {
                return new ServiceResponse(
                    isSuccess: false,
                    code: "EXPIRED_OTP",
                    userMessage: "El código ha expirado. Por favor solicite uno nuevo.",
                    detailMessage: "El código OTP ha expirado"
                );
            }
            catch (Exception ex)
            {
                return new ServiceResponse(
                    isSuccess: false,
                    code: "INTERNAL_ERROR",
                    userMessage: "Error del servicio, comunicarse con el administrador del sistema",
                    detailMessage: ex.Message
                );
            }
        }
    }
}
