

using SR.PasswordReset.Api.src.contexts.password_reset.application.dtos;

namespace SR.PasswordReset.Api.src.contexts.password_reset.application.interfaces.services
{
    public interface IResetPasswordService
    {
        Task<ServiceResponse> Execute(ResetPasswordApplicationDto request);
    }
}
