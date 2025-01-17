
using SR.PasswordReset.Api.src.contexts.password_reset.application.dtos;

namespace SR.PasswordReset.Api.src.contexts.password_reset.application.interfaces.services
{

    public interface IValidateOtpService
    {
        Task<ServiceResponse> Execute(ValidateOtpApplicationDto request);
    }
}
