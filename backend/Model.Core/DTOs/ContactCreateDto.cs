
namespace Model.Core.DTOs;

public class ContactCreateDto
{
    public string Name { get; set; }
    public string MobilePhone { get; set; }
    public string JobTitle { get; set; }
    public DateTime BirthDate { get; set; }
}