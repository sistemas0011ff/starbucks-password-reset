
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SR.PasswordReset.Api.src.contexts.shared.responses
{
    [JsonSerializable(typeof(BaseResponse))]
    public class BaseResponse
    {
        [JsonPropertyName("code")]
        [Required]
        public string Code { get; }

        [JsonPropertyName("userMessage")]
        [Required]
        public string UserMessage { get; }

        [JsonPropertyName("detailMessage")]
        [Required]
        public string DetailMessage { get; }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; }

        public BaseResponse(string code, string userMessage, string detailMessage, bool isSuccess)
        {
            Code = code;
            UserMessage = userMessage;
            DetailMessage = detailMessage;
            IsSuccess = isSuccess;
        }

        [JsonConstructor]
        public BaseResponse() { }
    }
}
