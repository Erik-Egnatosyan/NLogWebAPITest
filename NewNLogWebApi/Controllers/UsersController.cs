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
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _userService.GetUsers();
                if (result == null)
                {
                    throw new Exception("Ползователи не найдены!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetAllNames")]
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
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
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
                    throw new Exception("Данные по возростам не найдены!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
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
                if (result == null)
                {
                    throw new Exception("Ошибка при создании таблицы!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("AddUsers")]
        public async Task<ActionResult<string>> AddUserWithData()
        {
            try
            {
                var result = await _userService.AddUserWithData();
                if (result == null)
                {
                    throw new Exception("Ошибка при добовлении ползователья");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //-----------------------END-CREATE-----------------------
        //---------------------PUT-AND-PATCH----------------------

        [HttpPut("users/{id}/changenamePUT")]
        public async Task<IActionResult> ChangeNameWithPut(int id, string newName)
        {
            try
            {
                var result = await _userService.ChangeNameWithPut(id, newName);
                if (result == null)
                {
                    throw new Exception("Не удалось изменить имя пользователя");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        [HttpPatch("users/{id}/changenamePATCH")]
        public async Task<IActionResult> ChangeUserName(int id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
            try
            {
                var result = await _userService.ChangeUserName(id, patchDoc);
                if (result == null)
                {
                    throw new Exception("Не удалось изменить имя ползователя!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
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
            try
            {
                var result = await _userService.Delete(id);
                if (result == null)
                {
                    throw new Exception($"Не найден ползоваетль под номером {id}");
                }
                //return Ok("User deleted successfully!");
                return Ok(new { message = "User deleted successfully", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("DeleteTable")]
        public async Task<ActionResult> DropTable()
        {
            try
            {
                var result = _userService.DropTable();
                if (result == null)
                {
                    throw new Exception("Ошибка при удалении таблицы");
                }
                return Ok("Таблица успешно удалена!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("DeleteAllRowsFromLogs")]
        public async Task<IActionResult> DeleteAllRowsFromLogs()
        {
            try
            {
                var result = _userService.DeleteAllRowsFromLogs();
                if (result == null)
                {
                    throw new Exception("Ошибка при удалении всех строк из таблицы");
                }
                return Ok("Все строки из таблицы успешно удалены!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        //-----------------------END-DELETE-----------------------
    }
}
