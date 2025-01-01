using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class TokenService{
    private readonly string _secretKey = "yoursecretkey";

    public string GenerateToken(string username){
        var claims = new[]{
            new Claim(ClaimTypes.Name, username)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer : "yourissuer",
            audience : "audience",
            claims : claims,
            expires : DateTime.Now.AddHours(1),
            signingCredentials : credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}