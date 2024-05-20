using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskService.Models;
using Task = TaskService.Models.Task;

namespace TaskService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TaskController : ControllerBase
{
    private readonly TaskContext _context;

    public TaskController(TaskContext context)
    {
        _context = context;
    }

   [HttpGet]
public async Task<ActionResult<IEnumerable<Task>>> GetTasks([FromQuery]int userId)
{
    var tasks = await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
    return Ok(tasks);
}

    // GET: api/Tasks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Task>> GetTask(int id)
    {
        var Task = await _context.Tasks.FindAsync(id);

        if (Task == null)
        {
            return NotFound();
        }

        return Task;
    }

    // PUT: api/Tasks/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<Task>> PutTask(int id, Task Task)
    {
        if (id != Task.Id)
        {
            return BadRequest();
        }

        _context.Entry(Task).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TaskExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok(Task);
    }

    // POST: api/Tasks
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Task>> PostTask(Task Task)
    {
        _context.Tasks.Add(Task);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTask", new { id = Task.Id }, Task);
    }

    // DELETE: api/Tasks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TaskExists(int id)
    {
        return _context.Tasks.Any(e => e.Id == id);
    }
}