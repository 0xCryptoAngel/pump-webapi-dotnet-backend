namespace PUMP_BACKEND.Services.Interfaces
{ 
    public interface ITokenService
    {
        string GenerateJwt(string username, string tenant);
    }
}
