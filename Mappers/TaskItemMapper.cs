using To_Do_List.Dtos.Comment;
using To_Do_List.Dtos.Task;
using To_Do_List.Models;

namespace To_Do_List.Mappers
{
    public static class TaskItemMapper
    {
        // Entity -> DTO
        public static TaskItemReadDto ToReadDto(this TaskItem taskModel)
        {
            return new TaskItemReadDto
            {
                Id = taskModel.Id,
                Title = taskModel.Title,
                Description = taskModel.Description,
                IsCompleted = taskModel.IsCompleted,
                Comments = taskModel.Comments?.Select(c => c.ToReadDto()).ToList() ?? new List<CommentReadDto>()
            };
        }

        //DTO -> Entity(for creation)
        public static TaskItem ToEntity(this TaskItemCreateDto dto)
        {
            return new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false
            };
        }

        // Update existing TaskItem from DTO
        public static void UpdateEntity(this TaskItem task, TaskItemUpdateDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Title))
                task.Title = dto.Title;

            if (!string.IsNullOrEmpty(dto.Description))
                task.Description = dto.Description;

            if (dto.IsCompleted.HasValue)
                task.IsCompleted = dto.IsCompleted.Value;
        }
    }
}
