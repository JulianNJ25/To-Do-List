using To_Do_List.Dtos.Comment;
namespace To_Do_List.Dtos.Task
{
    public class TaskItemReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public List<CommentReadDto> Comments { get; set; } = new();
    }
}
