using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NewNLogWebApi.Models;

namespace NewNLogWebApi.Service
{
    public interface IUserService
    {
        Task<List<User>> GetUsers();
        Task<List<string>> GetAllNames();
        Task<double> AverageAge();
        Task<string> CreateTable();
        Task<string> AddUserWithData();
        Task<string> ChangeNameWithPut(int id, string newName);
        Task<string> ChangeUserName(int id, [FromBody] JsonPatchDocument<User> patchDoc);
        Task<User> Delete(int id);
        Task<string> DropTable();
        Task<string> DeleteAllRowsFromLogs();
    }
}
