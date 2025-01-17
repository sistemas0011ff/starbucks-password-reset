
using System.Text.RegularExpressions;

namespace SR.PasswordReset.Api.src.contexts.password_reset.domain.value_objects
{
    public class Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value), "El email no puede estar vacío");

            if (!IsValidEmail(value))
                throw new ArgumentException("El formato del email no es válido");

            return new Email(value);
        }

        private static bool IsValidEmail(string email)
        {
            // RFC 5322 Email validation
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (Email)obj;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator string(Email email) => email.Value;
    }
}
