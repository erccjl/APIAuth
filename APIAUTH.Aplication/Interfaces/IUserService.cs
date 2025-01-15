using APIAUTH.Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Aplication.Interfaces
{
    public interface IUserService 
    {
        Task<bool> Exists(int id);
        Task<UserDto> Save(CollaboratorDto dto);
        Task RecoverPassword(string email, string password);
        Task<bool> ChangePassword(UserDto dto);
    }
}
