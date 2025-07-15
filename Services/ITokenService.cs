namespace PUMP_BACKEND.Services
{ 
    public interface ITokenService
    {
        string GenerateJwt(string username, string tenant);
    }
}
