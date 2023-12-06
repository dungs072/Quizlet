using System.Security.Claims;

namespace QuizletWebMvc.Services.Token
{
    public interface ITokenService
    {
        string GenerateToken(string userId);
        ClaimsPrincipal ValidateToken(string token);
    }
}
