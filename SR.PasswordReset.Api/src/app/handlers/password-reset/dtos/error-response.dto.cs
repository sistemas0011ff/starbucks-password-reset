

namespace SR.PasswordReset.Api.src.app.handlers.password_reset.dtos
{
    public record ErrorResponseDto
    {
        public string Code { get; init; }
        public string UserMessage { get; init; }
        public string DetailMessage { get; init; }

        public ErrorResponseDto(string code, string userMessage, string detailMessage)
        {
            Code = code;
            UserMessage = userMessage;
            DetailMessage = detailMessage;
        }
    }
}
