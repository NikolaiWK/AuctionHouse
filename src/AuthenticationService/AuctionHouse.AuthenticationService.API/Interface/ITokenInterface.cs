using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuctionHouse.AuthenticationService.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AuctionHouse.AuthenticationService.API.Interface
{
    public interface ITokenService
    {
   
        Task<string> GenerateToken(User user);
    }

    public class TokenService(TokenConfig config) : ITokenService
    {
        public async Task<string> GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()) //Used for unique identifier for auction related bids
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = config.Issuer,
                Audience = config.Audience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }
    }
}
