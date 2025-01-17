

namespace SR.PasswordReset.Api.src.app.handlers.password_reset.dtos
{
    public record ValidateOtpRequestDto
    {
        public string Email { get; init; }
        public string Code { get; init; }

        public ValidateOtpRequestDto(string email, string code)
        {
            Email = email;
            Code = code;
        }
    }
}
