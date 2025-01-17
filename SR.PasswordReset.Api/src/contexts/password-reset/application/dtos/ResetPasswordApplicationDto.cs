
namespace SR.PasswordReset.Api.src.contexts.password_reset.application.dtos
{
    public record ResetPasswordApplicationDto
    {
        public string Email { get; init; }
        public string NewPassword { get; init; }

        public ResetPasswordApplicationDto(string email, string newPassword)
        {
            Email = email;
            NewPassword = newPassword;
        }
    }
}
