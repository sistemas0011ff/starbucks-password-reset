

using SR.PasswordReset.Api.src.contexts.password_reset.domain.entities;
using SR.PasswordReset.Api.src.contexts.password_reset.domain.value_objects;

namespace SR.PasswordReset.Api.src.contexts.password_reset.domain.repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsById(Email email);
        Task<User> GetById(Email email);
    }
}
