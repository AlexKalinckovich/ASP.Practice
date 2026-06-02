using Model.Core.DTOs;
using Model.Core.Models;

namespace Model.Core.Mappers
{
    public class ContactMapper : IContactMapper
    {
        public Contact MapToEntity(ContactCreateDto dto)
        {
            return new Contact
            {
                Name = dto.Name,
                MobilePhone = dto.MobilePhone,
                JobTitle = dto.JobTitle,
                BirthDate = dto.BirthDate
            };
        }

        public ContactReadDto MapToDto(Contact entity)
        {
            return new ContactReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                MobilePhone = entity.MobilePhone,
                JobTitle = entity.JobTitle,
                BirthDate = entity.BirthDate
            };
        }

        public void ApplyUpdate(ContactUpdateDto dto, Contact entity)
        {
            entity.Name = dto.Name;
            entity.MobilePhone = dto.MobilePhone;
            entity.JobTitle = dto.JobTitle;
            entity.BirthDate = dto.BirthDate;
        }

        public void ApplyPatch(ContactPatchDto dto, Contact entity)
        {
            PatchName(dto, entity);
            PatchMobilePhone(dto, entity);
            PatchJobTitle(dto, entity);
            PatchBirthDate(dto, entity);
        }

        private static void PatchName(ContactPatchDto dto, Contact entity)
        {
            if (dto.Name != null)
            {
                entity.Name = dto.Name;
            }
        }

        private static void PatchMobilePhone(ContactPatchDto dto, Contact entity)
        {
            if (dto.MobilePhone != null)
            {
                entity.MobilePhone = dto.MobilePhone;
            }
        }

        private void PatchJobTitle(ContactPatchDto dto, Contact entity)
        {
            if (dto.JobTitle != null)
            {
                entity.JobTitle = dto.JobTitle;
            }
        }

        private static void PatchBirthDate(ContactPatchDto dto, Contact entity)
        {
            if (dto.BirthDate.HasValue)
            {
                entity.BirthDate = dto.BirthDate.Value;
            }
        }
    }
}