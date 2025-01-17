using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.PasswordReset.Api.src.contexts.password_reset.application.dtos
{
    public record RequestOtpApplicationDto
    {
        public string Email { get; init; }

        public RequestOtpApplicationDto(string email)
        {
            Email = email;
        }
    }
}
