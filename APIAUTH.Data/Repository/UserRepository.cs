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
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _context;
        private DbSet<User> _dbSet;

        public UserRepository(AuthContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public async Task<User> Add(User item)
        {
            _context.Users.Add(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<User> Update(User item)
        {
            var localEntity = _dbSet.Local.FirstOrDefault(x => x.Id == item.Id);
            if (localEntity != null)
                _context.Entry(localEntity).State = EntityState.Detached;
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
            return item;
        }

        public async Task<User> Get(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public Collaborator GetByEmail(string email)
        {
            return  _context.Collaborators.Where(u => u.Email == email)
                .Include(u => u.CollaboratorType)
                .Include(u => u.Organization)
                .Include(u => u.User)
                .FirstOrDefault();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> ValidatePasswordAsync(User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public Collaborator GetCollaboratorByIdUser(int id)
        {
            return _context.Collaborators.Where(u => u.UserId == id)
                .Include(u => u.CollaboratorType)
                .Include(u => u.Organization)
                .FirstOrDefault();
        }

        public List<UserRole> GetRoles(int userId)
        {
            return _context.UserRoles
                     .Where(ur => ur.UserId == userId)
                     .Include(ur => ur.Role)
                     .ToList();
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

    }
}
