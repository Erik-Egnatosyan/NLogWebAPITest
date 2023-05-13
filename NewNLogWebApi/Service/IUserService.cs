using NewNLogWebApi.Models;

namespace NewNLogWebApi.Service
{
    public interface IUserService
    {
        Task<List<User>> GetUsers();
        Task<List<string>> GetAllNames();
    }
}
