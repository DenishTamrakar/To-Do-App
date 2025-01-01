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
    public readonly AppDbContext _context;

    public ToDosController(AppDbContext context){
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(){
        var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var todos = await _context.ToDos.Where(t => t.UserID == userID).ToListAsync();
        return Ok(todos);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TODO todo){
        todo.UserID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        _context.Add(todo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new {id = todo.TDID},todo);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, [FromBody] TODO updatedtodo){
        var todo = await _context.ToDos.FindAsync();
        if(todo == null || todo.UserID != int.Parse(User.FindFirst(ClaimTypes.Name)?.Value)){
            return NotFound();
        }
        todo.TDName = updatedtodo.TDName;
        todo.TDStatus = updatedtodo.TDStatus;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        var todo = await _context.ToDos.FindAsync(id);
        if(todo == null || todo.UserID != int.Parse(User.FindFirst(ClaimTypes.Name)?.Value)){
            return NotFound();
        }

        _context.ToDos.Remove(todo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}