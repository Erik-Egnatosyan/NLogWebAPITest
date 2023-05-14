using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewNLogWebApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                if (userModel is null)
                {
                    throw new ArgumentNullException(nameof(userModel), "User model cannot be null.");
                }
                _context.Users.Add(userModel);
                await _context.SaveChangesAsync();
                return "User added successfully!";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

        public async Task<string> ChangeNameWithPut(int id, string newName)
        {
            var user = await _context.Users.FindAsync(id);

            user.FirstName = newName;

            try
            {
                await _context.SaveChangesAsync();
                return "User name updated successfully!";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> ChangeUserName(int id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    throw new ArgumentNullException(nameof(patchDoc));
                }
                var user = await _context.Users.FindAsync(id);

                patchDoc.ApplyTo(user);

                await _context.SaveChangesAsync();

                return "User name updated successfully!";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> CreateTable()
        {
            try
            {
                string query = "Create table MyTable (id int PRIMARY KEY, name varchar(50))";
                if (string.IsNullOrWhiteSpace(query))
                {
                    throw new ArgumentNullException(nameof(query));
                }
                await _context.Database.ExecuteSqlRawAsync(query);
                return "Таблица успешно создана!";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> Delete(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                //var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    return null;
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteAllRowsFromLogs()
        {
            try
            {
                string query = "Truncate table Logs";
                if (string.IsNullOrWhiteSpace(query))
                {
                    throw new ArgumentNullException(nameof(query));
                }
                await _context.Database.ExecuteSqlRawAsync(query);
                return "Все строки из таблицы успешно удалены!";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DropTable()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("DROP TABLE MyTable");
                return "Таблица успешно удалена!";
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
            try
            {
                var users = await _context.Users.ToListAsync();
                if (users == null)
                {
                    return null;
                }
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
