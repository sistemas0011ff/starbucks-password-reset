

namespace SR.PasswordReset.Api.src.app.handlers.password_reset.dtos
{
    public record SendOtpRequestDto
    {
        public string Email { get; init; }

        public SendOtpRequestDto(string email)
        {
            Email = email;
        }
    }
}
