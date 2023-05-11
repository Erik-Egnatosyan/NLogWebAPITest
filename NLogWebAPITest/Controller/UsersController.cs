using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NLogWebAPITest.Models;

namespace NLogWebAPITest.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            using (var context = new NLogDBContext())
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpGet("Read")]
        public IEnumerable<User> GetUsers()
        {
            using (var context = new NLogDBContext())
            {
                return context.Users.ToList();
            }
        }

        [HttpGet("CheckDBConnection")] // атрибут для маршрутизации HTTP GET-запроса по адресу "api/CheckDBConnection"
        public ActionResult<string> CheckDBConnection()
        {
            // строка подключения к базе данных
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NLogDB;Trusted_Connection=True;";

            // объявление объекта SqlConnection в блоке using для автоматического закрытия подключения
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // открытие подключения к базе данных
                    connection.Open();

                    // если подключение успешно, возвращается строка "Connection successful!"
                    return "Connection successful!";
                }
                catch (Exception ex)
                {
                    // если произошла ошибка, возвращается строка "Connection failed: " с сообщением об ошибке
                    return $"Connection failed: {ex.Message}";
                }
            }
        }

        [HttpPut]
        public async Task<bool> UpdateUser(User user)
        {
            using (var context = new NLogDBContext())
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Email = user.Email;
                    existingUser.Phone = user.Phone;
                    existingUser.Address = user.Address;
                    existingUser.Age = user.Age;
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        [HttpDelete]
        public async Task<bool> DeleteUser(int id)
        {
            using (var context = new NLogDBContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                    context.Users.Remove(user);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

    }
}
