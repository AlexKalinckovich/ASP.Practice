namespace ContactBackend.Application.Core.Models;

public class Contact
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string MobilePhone { get; set; }
    public string JobTitle { get; set; }
    public DateTime BirthDate { get; set; }
}