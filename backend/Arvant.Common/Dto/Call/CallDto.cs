namespace Arvant.Common.Dto;

public class CallDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public Guid? InitiatorId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public double Duration { get; set; }
    public int Direction { get; set; }
}
