using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class User : BaseEntity
{
    public User() : base("USER")
    {
    }

    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string EmailId { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public bool IsActive { get; set; }
    public string? PasswordHash { get; set; }

}
