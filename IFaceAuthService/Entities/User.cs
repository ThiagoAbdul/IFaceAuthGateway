namespace IFaceAuthService.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string HashPassword { get; set; } = string.Empty;
    public bool Deleted { get; set; } = false;
    public bool AccountEnabled { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User()
    {
        
    }

    public User(string fullName, string email, string hashPassword)
    {
        FullName = fullName;
        Email = email;
        HashPassword = hashPassword;
    }
}
