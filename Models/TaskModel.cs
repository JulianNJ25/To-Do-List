namespace To_Do_List.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        // 1 -> N
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
