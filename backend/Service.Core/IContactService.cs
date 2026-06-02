using Model.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Core;

public interface IContactService
{
    Task<IEnumerable<ContactReadDto>> GetAllAsync();
    Task<ContactReadDto?> GetByIdAsync(long id);
    Task<ContactReadDto> CreateAsync(ContactCreateDto dto);
    Task UpdateAsync(ContactUpdateDto dto);
    Task PatchAsync(long id, ContactPatchDto dto);
    Task DeleteAsync(long id);
}