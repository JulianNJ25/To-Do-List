using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do_List.Data;       
using To_Do_List.Dtos.Comment;
using To_Do_List.Mappers;
using To_Do_List.Models;

namespace To_Do_List.Controllers
{
    [Authorize]
    [ApiController]
    [Route("to-do-list/tasks/{taskItemId}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/tasks/{taskItemId}/comments
        [HttpPost]
        public async Task<ActionResult<CommentReadDto>> CreateComment(int taskItemId, CommentCreateDto dto)
        {
            var task = await _context.TaskItems
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == taskItemId);

            if (task is null)
                return NotFound($"Task with id {taskItemId} not found.");

            var comment = dto.ToEntity(taskItemId);

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCommentsByTask),
                new { taskItemId },
                comment.ToReadDto()
            );
        }

        // endpoint for replies
        [HttpPost("/to-do-list/comments/{parentCommentId}/reply")]
        public async Task<ActionResult<CommentReadDto>> ReplyToComment(int parentCommentId, CommentCreateDto dto)
        {
            var parent = await _context.Comments.FindAsync(parentCommentId);
            if (parent is null)
                return NotFound($"Parent comment {parentCommentId} not found.");

            var reply = dto.ToEntity(parent.TaskItemId, parentCommentId);
            _context.Comments.Add(reply);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommentsByTask), new { taskItemId = parent.TaskItemId }, reply.ToReadDto());
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentReadDto>>> GetCommentsByTask(int taskItemId)
        {
            // Ensure task exists
            if (!await _context.TaskItems.AnyAsync(t => t.Id == taskItemId))
                return NotFound($"Task with id {taskItemId} not found.");

            // Load only root comments (no parent)
            var comments = await _context.Comments
                .Include(c => c.Replies)
                .Where(c => c.TaskItemId == taskItemId && c.ParentCommentId == null)
                .ToListAsync();

            // Map and return
            var commentDtos = comments.Select(c => c.ToReadDto()).ToList();
            return Ok(commentDtos);
        }

        [HttpPut("/to-do-list/comments/{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, CommentCreateDto dto)
        {
            // Look up the comment by ID
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment is null)
                return NotFound($"Comment with id {commentId} not found.");

            // Update comment text and mark as updated
            comment.UpdateEntity(dto);

            // Save changes
            await _context.SaveChangesAsync();

            // Return 204 No Content (standard for successful PUT)
            return NoContent();
        }

        [HttpDelete("/to-do-list/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _context.Comments
                .Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
                return NotFound($"Comment with id {commentId} not found.");

            await DeleteCommentRecursive(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private async Task DeleteCommentRecursive(Comment comment)
        {
            await _context.Entry(comment).Collection(c => c.Replies).LoadAsync();

            foreach (var reply in comment.Replies.ToList())
            {
                await DeleteCommentRecursive(reply);
            }

            _context.Comments.Remove(comment);
        }


    }
}
