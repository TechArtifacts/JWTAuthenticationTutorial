using JWTAuthenticationTutorial.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthenticationTutorial
{
    public class AppAuthService : IAppAuthService
    {
        private readonly IConfiguration _configuration;

        Dictionary<string, string> TestUsers = new Dictionary<string, string>
        {
            { "test_user_1","password@123"}
        };

        public AppAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Token> Authenticate(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                throw new Exception("Invalid Input received!");

            if (!TestUsers.ContainsKey(user.Username))
                throw new Exception($"User with name {user.Username} not found!");

            if (user.Password != TestUsers[user.Username])
                throw new Exception("Invalid Password!");

            // User name and password are valid. 
            // Generate JSON Web Token

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Token { AuthToken = tokenHandler.WriteToken(token) };
        }
    }
}
