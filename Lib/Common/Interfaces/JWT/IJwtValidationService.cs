namespace TagTeamdWeb.Common.Interfaces.Jwt
{
    public interface IJwtValidationService
    {
        Task<bool> ValidateTokenAsync(string username, string token);
    }
}
