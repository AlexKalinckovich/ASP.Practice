using Model.Core.DTOs;

namespace Model.Core.Mappers;

public interface IContactMapper
{
    Models.Contact MapToEntity(ContactCreateDto dto);
    ContactReadDto MapToDto(Models.Contact entity);
    void ApplyUpdate(ContactUpdateDto dto, Models.Contact entity);
    void ApplyPatch(ContactPatchDto dto, Models.Contact entity);
}