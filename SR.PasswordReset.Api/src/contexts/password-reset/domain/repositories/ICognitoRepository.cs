
using SR.PasswordReset.Api.src.contexts.password_reset.domain.value_objects;

namespace SR.PasswordReset.Api.src.contexts.password_reset.domain.repositories
{
    public interface ICognitoRepository
    {
        Task ConfirmUserExists(Email email);
        Task<bool> ValidateOtp(Email email, string code);
        Task SendOtp(Email email);
        Task ResetPassword(Email email, string newPassword);
    }
}
