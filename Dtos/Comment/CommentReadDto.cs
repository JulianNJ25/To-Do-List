namespace To_Do_List.Dtos.Comment
{
    public class CommentReadDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public bool IsUpdated { get; set; }
        public int? ParentCommentId { get; set; }
        public List<CommentReadDto> Replies { get; set; } = new();
    }
}
