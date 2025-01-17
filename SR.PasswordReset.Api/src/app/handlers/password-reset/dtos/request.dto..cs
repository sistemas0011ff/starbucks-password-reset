using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.PasswordReset.Api.src.app.handlers.password_reset.dtos
{
    public record RequestOtpRequestDto(string Email);
    public record RequestOtpResponseDto(string Message, bool IsValid);
}
