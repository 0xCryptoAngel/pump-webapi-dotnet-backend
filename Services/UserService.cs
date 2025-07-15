using PUMP_BACKEND.Entities;

namespace PUMP_BACKEND.Services
{ 
    public class UserService : IUserService
    {
        public User? GetUserByUsername(string tenant, string username)
        {
            if (tenant == "tenant1" && username == "admin")
            {
                return new User
                {
                    Username = "admin",
                    Password = "password",
                    Email = "admin@tenant1.com"
                }; 
            }

            return null;
        }
    }

}
