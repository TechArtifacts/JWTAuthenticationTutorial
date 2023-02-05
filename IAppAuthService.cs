using JWTAuthenticationTutorial.Models;

namespace JWTAuthenticationTutorial
{
    public interface IAppAuthService
    {
        Task<Token> Authenticate(User user);
    }
}
