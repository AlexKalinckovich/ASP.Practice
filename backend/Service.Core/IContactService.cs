
using Model.Core.DTOs;

namespace Service.Core;

public interface IContactService
{
    IEnumerable<ContactReadDto> GetAll();
    ContactReadDto? GetById(long id);
    ContactReadDto Create(ContactCreateDto dto);
    void Update(ContactUpdateDto dto);
    void Patch(long id, ContactPatchDto dto);
    void Delete(long id);
}