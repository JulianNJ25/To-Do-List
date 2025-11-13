using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using To_Do_List.Data;
using To_Do_List.Dtos.Task;
using To_Do_List.Mappers;
using To_Do_List.Models;
namespace To_Do_List.Controllers
{
    [Authorize]
    [Route("to-do-list/task")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TaskItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var taskItems = _context.TaskItems.ToList()
                .Select(s => TaskItemMapper.ToReadDto(s));
            return Ok(taskItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var taskItem = _context.TaskItems.Find(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return Ok(taskItem.ToReadDto());
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemReadDto>> CreateTask(TaskItemCreateDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, new TaskItemReadDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItemUpdateDto dto)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task is null) return NotFound();

            if (dto.Title is not null)
                task.Title = dto.Title;
            if (dto.Description is not null)
                task.Description = dto.Description;
            if (dto.IsCompleted.HasValue)
                task.IsCompleted = dto.IsCompleted.Value;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task is null) return NotFound();

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }



    }
}
