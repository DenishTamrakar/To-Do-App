using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ToDosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ToDosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userIDClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(string.IsNullOrEmpty(userIDClaim)){
            return Unauthorized("User ID claim is missing");
        }
        var userID = int.Parse(userIDClaim);
        var todos = await _context.ToDos.Where(t => t.UserID == userID).ToListAsync();
        return Ok(todos);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TODO todo)
    {
        var userIDClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIDClaim))
        {
            return Unauthorized("User ID claim is missing");
        }
        todo.UserID = int.Parse(userIDClaim);
        
        _context.ToDos.Add(todo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = todo.TDID }, todo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TODO updatedtodo)
    {
        var todo = await _context.ToDos.FindAsync(id);
        if (todo == null)
            return NotFound();

        var userIDClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIDClaim) || todo.UserID != int.Parse(userIDClaim))
        {
            return Unauthorized("User ID mismatch");
        }

        todo.TDName = updatedtodo.TDName;
        todo.TDStatus = updatedtodo.TDStatus;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var todo = await _context.ToDos.FindAsync(id);
        if (todo == null)
            return NotFound();

        var userIDClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIDClaim) || todo.UserID != int.Parse(userIDClaim))
        {
            return Unauthorized("User ID mismatch");
        }

        _context.ToDos.Remove(todo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
