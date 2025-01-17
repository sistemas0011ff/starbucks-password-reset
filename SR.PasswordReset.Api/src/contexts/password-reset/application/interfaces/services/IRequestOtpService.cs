

using SR.PasswordReset.Api.src.contexts.password_reset.application.dtos;

namespace SR.PasswordReset.Api.src.contexts.password_reset.application.services
{
    public interface IRequestOtpService
    {
        Task<ServiceResponse> Execute(RequestOtpApplicationDto request);
    }
}
