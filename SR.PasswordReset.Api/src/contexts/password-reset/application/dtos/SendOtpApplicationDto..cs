
namespace SR.PasswordReset.Api.src.contexts.password_reset.application.dtos
{
    public record SendOtpApplicationDto
    {
        public string Email { get; init; }

        public SendOtpApplicationDto(string email)
        {
            Email = email;
        }
    }
}
