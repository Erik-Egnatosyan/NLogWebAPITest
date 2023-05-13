using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewNLogWebApi.Models;
using NewNLogWebApi.Service;

namespace NewNLogWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersContext _context;
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        public UsersController(ILogger<UsersController> logger, UsersContext context, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
        }
        //--------------------------READ--------------------------
        [HttpGet("Read")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _userService.GetUsers();
                if (result == null)
                {
                    throw new Exception("Ползователи не найдены");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return new NotFoundObjectResult(new
                {
                    Status = 404,
                    Message = "Not Found"
                });
            }
        }

        [HttpGet("GetOnlyNames")]
        public async Task<IActionResult> GetAllNames()
        {
            try
            {
                var result = await _userService.GetAllNames();
                if (result == null)
                {
                    throw new Exception("Данные не найдены");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return new NotFoundObjectResult(new
                {
                    Status = 404,
                    Message = "Not Found"
                });
            }
        }

        [HttpGet("AVG_Age")]
        public async Task<ActionResult> AverageAge()
        {
            try
            {
                var result = await _userService.AverageAge();
                if (result == 0)
                {
                    throw new Exception("Данные не найдены");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex.Message);
                return new NotFoundObjectResult(new
                {
                    Status = 404,
                    Message = "Not Found"
                });
            }
        }

        //------------------------END-READ------------------------
        //-------------------------CREATE-------------------------
        [HttpPost("CreateTable")]
        public async Task<IActionResult> CreateTable()
        {
            try
            {
                var result = await _userService.CreateTable();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании таблицы");
                return BadRequest($"Ошибка при создании таблицы: {ex.Message}");
            }
        }
        [HttpPost("AddUsers")]
        public async Task<ActionResult<string>> AddUserWithData()
        {
            try
            {
                var result = await _userService.AddUserWithData();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user");
                return BadRequest($"Error adding user: {ex.Message}");
            }
        }

        //-----------------------END-CREATE-----------------------
        //---------------------PUT-AND-PATCH----------------------
        [HttpPut("ChangeInformation")]
        public async Task<ActionResult<string>> ChangeNameWithPut(int id, string newName)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Запрошенный пользователь не найден");
                return NotFound();
            }

            user.FirstName = newName;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("User name updated successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user name");
                return BadRequest($"Error updating user name: {ex.Message}");
            }
        }
        [HttpPatch("users/{id}/changename")]
        public async Task<IActionResult> ChangeUserName(int id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null) { _logger.LogWarning("Запрошенный пользователь не найден"); return NotFound(); }

                patchDoc.ApplyTo(user);

                await _context.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return BadRequest($"Error updating user: {ex.Message}");
            }
            //operationType: целое число, которое определяет тип операции (0 - add, 1 - remove, 2 - replace, 3 - move, 4 - copy, 5 - test).
            //path: строка, которая указывает на путь к свойству, которое нужно изменить.
            //op: строка, которая определяет тип операции(add, remove, replace, move, copy, test).
            //from: строка, которая указывает на исходное положение свойства при операции move или copy.
            //value: значение, на которое нужно заменить свойство при операции replace или add.
            //[  {    "operationType": 2,    "path": "/FirstName",    "op": "replace",    "value": "John"  }]
        }
        //-------------------END-PUT-AND-PATCH--------------------
        //-------------------------DELETE-------------------------
        [HttpDelete("DeleteUserById/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Запрошенный пользователь не найден");
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok("User deleted successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return BadRequest($"Error deleting user: {ex.Message}");
            }
        }
        [HttpDelete("DeleteTable")]
        public async Task<ActionResult> DropTable()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("DROP TABLE Users");
                return Ok("Таблица успешно удалена!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении таблицы");
                return BadRequest($"Ошибка при удалении таблицы: {ex.Message}");
            }
        }
        [HttpDelete("DeleteAllRowsFromLogs")]
        public async Task<IActionResult> DeleteAllRowsFromLogs()
        {
            try
            {
                string query = "Truncate table Logs";
                await _context.Database.ExecuteSqlRawAsync(query);
                return Ok("Все строки из таблицы успешно удалены!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении всех строк из таблицы: {ex.Message}");
                return BadRequest($"Ошибка при удалении всех строк из таблицы: {ex.Message}");
            }
        }
        //-----------------------END-DELETE-----------------------
    }
}
