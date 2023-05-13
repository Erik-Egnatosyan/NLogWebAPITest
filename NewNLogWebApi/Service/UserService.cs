using Microsoft.EntityFrameworkCore;
using NewNLogWebApi.Models;

namespace NewNLogWebApi.Service
{
    public class UserService : IUserService
    {
        private readonly UsersContext _context;
        public UserService(UsersContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetAllNames()
        {
            try
            {
                var users = await _context.Users.ToArrayAsync();
                if (users == null)
                {
                    return null;
                }
                var names = new List<string>();

                foreach (var user in users)
                {
                    names.Add(user.FirstName);
                }
                return names;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null)
            {
                return null;
            }
            return users;
        }
    }
}
