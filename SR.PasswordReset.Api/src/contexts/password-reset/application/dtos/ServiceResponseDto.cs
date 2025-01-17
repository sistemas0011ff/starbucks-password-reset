
namespace SR.PasswordReset.Api.src.contexts.password_reset.application.dtos
{
    public class ServiceResponse
    {
        public bool IsSuccess { get; init; }
        public string Code { get; init; }
        public string UserMessage { get; init; }
        public string DetailMessage { get; init; }

        public ServiceResponse(bool isSuccess, string code, string userMessage, string detailMessage)
        {
            IsSuccess = isSuccess;
            Code = code;
            UserMessage = userMessage;
            DetailMessage = detailMessage;
        }
    }
}
