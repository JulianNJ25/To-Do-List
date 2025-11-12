using Microsoft.EntityFrameworkCore;
using To_Do_List.Models;

namespace To_Do_List.Data
{
    public class ApplicationDBContext : DbContext
    {
        // database constructor 
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<TaskItem> TaskItems { get; set; }
        
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
