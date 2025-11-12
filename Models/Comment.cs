namespace To_Do_List.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public bool IsUpdated { get; set; }
        public int TaskItemId { get; set; }
        // optional in case we do not need to load the full task
        public TaskItem? TaskItem { get; set; }
        // self-referencing relationship, holds ID for the comment being replied to
        // nullable, parent comments are not replies
        // threaded comment structure
        public int? ParentCommentId { get; set; }
        // navigation propert to the parent comment
        // access the comment being replied to directly 
        public Comment? ParentComment { get; set; }
       // N -> 1
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}