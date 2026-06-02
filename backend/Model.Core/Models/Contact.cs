namespace Model.Core.Models;

public class Contact
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MobilePhone { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}