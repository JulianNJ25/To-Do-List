using To_Do_List.Dtos.Comment;
using To_Do_List.Models;

namespace To_Do_List.Mappers
{
    public static class CommentMapper
    {
        // Entity -> DTO
        public static CommentReadDto ToReadDto(this Comment comment)
        {
            return new CommentReadDto
            {
                Id = comment.Id,
                CommentText = comment.CommentText,
                IsUpdated = comment.IsUpdated,
                ParentCommentId = comment.ParentCommentId,
                Replies = comment.Replies?.Select(r => r.ToReadDto()).ToList() ?? new List<CommentReadDto>()
            };
        }

        // DTO -> Entity (for creation)
        public static Comment ToEntity(this CommentCreateDto dto, int taskItemId, int? parentCommentId = null)
        {
            return new Comment
            {
                CommentText = dto.CommentText,
                TaskItemId = taskItemId,
                ParentCommentId = parentCommentId // explicitly ensure it's a parent comment
            };
        }

        // Update existing Comment from DTO
        public static void UpdateEntity(this Comment comment, CommentCreateDto dto)
        {
            comment.CommentText = dto.CommentText;
            comment.IsUpdated = true;
        }
    }
}
