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

        public async Task<string> AddUserWithData()
        {
            try
            {
            User userModel = CreateUserModel("Erik", "Nalbandyan", "eriknalb@mail.com", "+87987546546", "Pushkinskiy", 17);
                _context.Users.Add(userModel);
                await _context.SaveChangesAsync();
                return "User added successfully!";
            }
            catch (Exception)
            {

                throw;
            }      
            User CreateUserModel(string FirstName, string LastName, string Email, string Phone, string Address, int Age)
            {
                return new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Phone = Phone,
                    Address = Address,
                    Age = Age
                };
            }
        }

        public async Task<double> AverageAge()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null)
            {
                return 0;
            }
            var averageAge = users.Average(user => user.Age);

            return averageAge;
        }

        public async Task<string> CreateTable()
        {
            try
            {
                string query = "Create table MyTable (id int PRIMARY KEY, name varchar(50))";
                await _context.Database.ExecuteSqlRawAsync(query);
                return "Таблица успешно создана!";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
