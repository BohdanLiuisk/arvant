namespace Arvant.Common.Dto;

public class ParticipantForCallDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public bool IsOnline { get; set; }
    public string AvatarUrl { get; set; }
}
