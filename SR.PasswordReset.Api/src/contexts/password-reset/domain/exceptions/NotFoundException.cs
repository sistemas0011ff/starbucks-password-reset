

namespace SR.PasswordReset.Api.src.contexts.password_reset.domain.exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
