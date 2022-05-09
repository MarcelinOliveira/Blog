using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BlogVisualStudio.Models;
using Microsoft.IdentityModel.Tokens;

namespace BlogVisualStudio.Services;

public class TokenService
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}