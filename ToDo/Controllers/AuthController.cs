using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;
namespace ToDo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenservice;
    private readonly AppDbContext _context;
    public AuthController(TokenService tokenService, AppDbContext context){
        _tokenservice = tokenService;
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel login)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == login.Username);
        
        // If the user doesn't exist or the password doesn't match, return Unauthorized
        if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid Credentials");
        }

        // Generate the JWT token with user details
        var token = _tokenservice.GenerateToken(user.Username, user.UserID);

        // Return the token in the response
        return Ok(new { Token = token });
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserLoginModel register){
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == register.Username);
        if(existingUser != null){
            return BadRequest("Username already exists");
        }
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(register.Password);
        var newUser = new User{
            Username = register.Username,
            PasswordHash = passwordHash
        };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        var token = _tokenservice.GenerateToken(newUser.Username, newUser.UserID);
        return Ok(new{Token = token});
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(){
        var users = await _context.Users.ToListAsync();
        if (users == null || !users.Any())
        {
            return NotFound("No users found.");
        }
        return Ok(users);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        var user = await _context.Users.FindAsync(id);
        if (user == null)
    {
        return NotFound("User not found.");
    }
    _context.Users.Remove(user);
    await _context.SaveChangesAsync();
    return NoContent();
    }
}
