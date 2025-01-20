using APIAUTH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Domain.Repository
{
    public interface IUserRepository
    {
        Task<User> Add(User item);
        Task<User> Update(User item);
        Task<User> Get(int id);
        Collaborator GetByEmail(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<bool> ValidatePasswordAsync(User user, string password);
        Collaborator GetCollaboratorByIdUser(int id);
        List<Role> GetRoles();
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
    }
}
