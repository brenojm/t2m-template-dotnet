namespace T2MTemplate.Domain.Entities;

public class Collaborator
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
}
