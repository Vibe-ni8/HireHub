using HireHub.Shared.Common.Models;

namespace HireHub.Core.DTO;

public class LoginResponse : BaseResponse
{
    public string Data { get; set; } = null!;
}
