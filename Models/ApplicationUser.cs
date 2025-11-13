using Microsoft.AspNetCore.Identity;

namespace To_Do_List.Models
{
    // Extends the built-in IdentityUser class (which already includes: Id, UserName, Email, PasswordHash, etc.)
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}

