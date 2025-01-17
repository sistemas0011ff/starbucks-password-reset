
namespace SR.PasswordReset.Api.src.contexts.password_reset.domain.exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message)
        {
        }
    }
}
