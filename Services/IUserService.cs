using PUMP_BACKEND.Entities;
namespace PUMP_BACKEND.Services
{
    public interface IUserService
    {
        User? GetUserByUsername(string tenant, string username);
    }
}