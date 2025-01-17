
using SR.PasswordReset.Api.src.contexts.password_reset.application.dtos;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.repositories;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.value_objects;

namespace SR.PasswordReset.Api.src.contexts.password_reset.application.services
{
    public class RequestOtpService : IRequestOtpService
    {
        private readonly IUserRepository _userRepository;

        public RequestOtpService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse> Execute(RequestOtpApplicationDto request)
        {
            try
            {
                var email = Email.Create(request.Email);
                var userExists = await _userRepository.ExistsById(email);

                if (!userExists)
                {
                    return new ServiceResponse(
                        isSuccess: false,
                        code: "USER_NOT_FOUND",
                        userMessage: "El usuario no está registrado en el sistema.",
                        detailMessage: $"No se encontró el usuario con email {email.Value}"
                    );
                }

                return new ServiceResponse(
                    isSuccess: true,
                    code: "REQUEST_OTP_SUCCESS",
                    userMessage: "Usuario verificado correctamente.",
                    detailMessage: $"Se ha verificado el usuario con email {email.Value}"
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
