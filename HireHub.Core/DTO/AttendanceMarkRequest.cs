namespace HireHub.Core.DTO;

public class AttendanceMarkRequest
{
    public int CandidateId { get; set; }
    public int UserSlotId { get; set; }
    public bool IsPresent { get; set; }
}
