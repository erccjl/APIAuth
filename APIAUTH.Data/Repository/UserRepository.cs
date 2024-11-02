using APIAUTH.Data.Context;
using APIAUTH.Domain.Entities;
using APIAUTH.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Data.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AuthContext _authContext;

        public UserRepository(AuthContext context) : base(context)
        {
            _authContext = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _authContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _authContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }


        public async Task<bool> ValidatePasswordAsync(User user, string password)
        {
            // Usar una biblioteca como BCrypt o alguna estrategia hash
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
    }
}
