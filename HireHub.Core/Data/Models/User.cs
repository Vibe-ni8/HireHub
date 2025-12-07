using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class User : BaseEntity
{
    public User() : base("USER")
    {
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string EmailId { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public bool IsActive { get; set; }
    public string? PasswordHash { get; set; }
    public virtual ICollection<UserSlot> UserSlots { get; set; } = [];

}
