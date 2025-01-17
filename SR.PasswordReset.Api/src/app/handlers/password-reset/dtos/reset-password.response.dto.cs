
namespace SR.PasswordReset.Api.src.app.handlers.password_reset.dtos
{
    public record ResetPasswordResponseDto
    {
        public string Message { get; init; }
        public bool IsValid { get; init; }

        public ResetPasswordResponseDto(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }
}
