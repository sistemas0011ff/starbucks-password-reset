
using SR.PasswordReset.Api.src.contexts.password_reset.domain.value_objects;

namespace SR.PasswordReset.Api.src.contexts.password_reset.domain.entities
{
    public class User
    {
        public Email Email { get; private set; }
        public bool IsActive { get; private set; }

        private User(Email email)
        {
            Email = email;
            IsActive = true;
        }

        public static User Create(string email)
        {
            var emailVO = Email.Create(email);
            return new User(emailVO);
        }

        // Métodos de dominio que podrían ser necesarios para el reset de password (No lo considero el uso par ael reto)
        public void DeactivateAccount()
        {
            IsActive = false;
        }

        public void ActivateAccount()
        {
            IsActive = true;
        }
    }
}
