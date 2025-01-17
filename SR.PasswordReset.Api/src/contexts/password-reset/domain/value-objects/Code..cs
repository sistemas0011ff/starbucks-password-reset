

using System.Text.RegularExpressions;

namespace SR.PasswordReset.Api.src.contexts.password_reset.domain.value_objects
{
    public class Code
    {
        public string Value { get; init; }

        private Code(string value)
        {
            Value = value;
        }

        public static Code Create(string value)
        {
            Validate(value);
            return new Code(value);
        }

        private static void Validate(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("El código no puede estar vacío");

            if (value.Length < 6 || value.Length > 6)  // Los códigos OTP típicamente son de 6 dígitos
                throw new ArgumentException("El código debe ser de 6 dígitos");

            if (!Regex.IsMatch(value, @"^\d{6}$"))  // Solo dígitos
                throw new ArgumentException("El código debe contener solo números");
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
