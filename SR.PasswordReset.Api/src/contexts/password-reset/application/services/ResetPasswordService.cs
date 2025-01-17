

using Amazon.CognitoIdentityProvider.Model;
using SR.PasswordReset.Api.src.contexts.password_reset.application.dtos;
using SR.PasswordReset.Api.src.contexts.password_reset.application.interfaces.services;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.repositories;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.value_objects;

namespace SR.PasswordReset.Api.src.contexts.password_reset.application.services
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly ICognitoRepository _cognitoRepository;
        private readonly IUserRepository _userRepository;

        public ResetPasswordService(
            ICognitoRepository cognitoRepository,
            IUserRepository userRepository)
        {
            _cognitoRepository = cognitoRepository;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse> Execute(ResetPasswordApplicationDto request)
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

                // 2. Resetear contraseña en Cognito
                await _cognitoRepository.ResetPassword(email, request.NewPassword);

                return new ServiceResponse(
                    isSuccess: true,
                    code: "RESET_PASSWORD_SUCCESS",
                    userMessage: "Contraseña actualizada correctamente",
                    detailMessage: $"Se actualizó exitosamente la contraseña para el usuario {email.Value}"
                );
            }
            catch (InvalidPasswordException)
            {
                return new ServiceResponse(
                    isSuccess: false,
                    code: "INVALID_PASSWORD",
                    userMessage: "La contraseña no cumple con los requisitos de seguridad",
                    detailMessage: "La contraseña debe cumplir con los requisitos mínimos de seguridad"
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
