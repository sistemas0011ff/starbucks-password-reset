

namespace SR.PasswordReset.Api.src.app.handlers.password_reset.dtos
{
    public record ValidateOtpResponseDto
    {
        public string Message { get; init; }
        public bool IsValid { get; init; }

        public ValidateOtpResponseDto(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }
}
