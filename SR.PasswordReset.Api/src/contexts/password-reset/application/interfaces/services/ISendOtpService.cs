

using SR.PasswordReset.Api.src.contexts.password_reset.application.dtos;

namespace SR.PasswordReset.Api.src.contexts.password_reset.application.interfaces.services
{
    public interface ISendOtpService
    {
        Task<ServiceResponse> Execute(SendOtpApplicationDto request);
    }
}
