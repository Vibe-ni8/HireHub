using HireHub.Core.Data.Models;
using HireHub.Shared.Common.Models;

namespace HireHub.Core.DTO;

public class Response<T> : BaseResponse where T : class
{
    public T? Data { get; set; } = null;
}

public class AdminDashboardDetails
{
    public int TotalUsers { get; set; }
    public int TotalCandidates { get; set; }
    public int TotalPanelMembers { get; set; }
    public int TotalMentors { get; set; }
    public int TotalHrs { get; set; }
    public int TotalInterviews { get; set; }
    public int TotalCandidatesHired { get; set; }
    public int TotalCandidatesRejected { get; set; }
}