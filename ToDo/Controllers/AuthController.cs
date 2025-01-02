using Microsoft.AspNetCore.Mvc;
using ToDo.Models;
namespace ToDo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenservice;

    public AuthController(TokenService tokenService){
        _tokenservice = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login ([FromBody] UserLoginModel login){
        if(login.Username == "test" && login.Password == "password"){
            var token = _tokenservice.GenerateToken(login.Username);
            return Ok(new {Token = token});
        }
        return Unauthorized("");
    }
}
