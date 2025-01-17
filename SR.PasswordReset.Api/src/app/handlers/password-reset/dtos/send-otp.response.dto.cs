
namespace SR.PasswordReset.Api.src.app.handlers.password_reset.dtos
{
    public record SendOtpResponseDto
    {
        public string Message { get; init; }
        public bool IsValid { get; init; }

        public SendOtpResponseDto(string message, bool isValid)
        {
            Message = message;
            IsValid = isValid;
        }
    }
}
