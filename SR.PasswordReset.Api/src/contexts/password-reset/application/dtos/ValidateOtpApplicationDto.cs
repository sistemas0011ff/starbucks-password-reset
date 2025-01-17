

namespace SR.PasswordReset.Api.src.contexts.password_reset.application.dtos
{
    public record ValidateOtpApplicationDto
    {
        public string Email { get; init; }
        public string Code { get; init; }

        public ValidateOtpApplicationDto(string email, string code)
        {
            Email = email;
            Code = code;
        }
    }
}
