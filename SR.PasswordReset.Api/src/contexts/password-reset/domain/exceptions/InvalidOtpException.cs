
namespace SR.PasswordReset.Api.src.contexts.password_reset.domain.exceptions
{
    public class InvalidOtpException : Exception
    {
        public InvalidOtpException(string message) : base(message)
        {
        }
    }
}
