
namespace SR.PasswordReset.Api.src.app.handlers.password_reset.dtos
{
    public record ResetPasswordRequestDto
    {
        public string Email { get; init; }
        public string NewPassword { get; init; }

        public ResetPasswordRequestDto(string email, string newPassword)
        {
            Email = email;
            NewPassword = newPassword;
        }
    }
}
