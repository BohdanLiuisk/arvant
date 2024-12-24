namespace Arvant.Common.Dto.CallHistory;

public class CallParticipantHistoryDto
{
    public int CallParticipantId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public bool IsOnline { get; set; }
    public int CallDirection { get; set; }
    public int Status { get; set; }
}
