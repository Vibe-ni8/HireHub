namespace HireHub.Core.DTO;

public class LoginRequest
{
    /// <summary>
    /// Accept email only here
    /// </summary>
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
