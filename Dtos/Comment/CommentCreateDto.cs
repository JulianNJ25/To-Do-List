namespace To_Do_List.Dtos.Comment
{
    public class CommentCreateDto
    {
        public string CommentText { get; set; } = string.Empty;
        public int? ParentCommentId { get; set; } //optional replies
    }
}
