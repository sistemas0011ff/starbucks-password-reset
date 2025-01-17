
using SR.PasswordReset.Api.src.contexts.password_reset.application.dtos;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.exceptions;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.repositories;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.value_objects;

namespace SR.PasswordReset.Api.src.contexts.password_reset.application.interfaces.services
{
    public class SendOtpService : ISendOtpService
    {
        private readonly ICognitoRepository _cognitoRepository;
        private readonly IUserRepository _userRepository;

        public SendOtpService(
            ICognitoRepository cognitoRepository,
            IUserRepository userRepository)
        {
            _cognitoRepository = cognitoRepository;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse> Execute(SendOtpApplicationDto request)
        {
            try
            {
                var email = Email.Create(request.Email);

                // 1. Verificar que el usuario existe
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

                // 2. Enviar OTP a través de Cognito
                await _cognitoRepository.SendOtp(email);

                return new ServiceResponse(
                    isSuccess: true,
                    code: "SEND_OTP_SUCCESS",
                    userMessage: $"Se ha enviado un código de verificación al correo {email.Value}",
                    detailMessage: $"Código OTP enviado exitosamente al usuario {email.Value}"
                );
            }
            catch (Exception ex)
            {
                return new ServiceResponse(
                    isSuccess: false,
                    code: "INTERNAL_ERROR",
                    userMessage: "Error al enviar el código de verificación",
                    detailMessage: ex.Message
                );
            }
        }
    }
}
