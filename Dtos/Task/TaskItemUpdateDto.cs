namespace To_Do_List.Dtos.Task
{
    public class TaskItemUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
