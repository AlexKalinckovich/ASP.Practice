using Data.Core;
using Model.Core.DTOs;
using Model.Core.Mappers;
using Model.Core.Models;
using Validation.Core;

namespace Service.Core;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;
    private readonly IContactMapper _mapper;
    private readonly IValidationFactory _validationFactory;

    public ContactService(
        IContactRepository repository,
        IContactMapper mapper,
        IValidationFactory validationFactory)
    {
        _repository = repository;
        _mapper = mapper;
        _validationFactory = validationFactory;
    }

    public IEnumerable<ContactReadDto> GetAll()
    {
        IEnumerable<Contact> entities = _repository.GetAll();
        return entities.Select(MapEntityToDto).ToList();
    }

    private ContactReadDto MapEntityToDto(Contact entity)
    {
        return _mapper.MapToDto(entity);
    }

    public ContactReadDto? GetById(long id)
    {
        Contact? entity = _repository.GetById(id);
        return ProcessGetByIdResult(entity);
    }

    private ContactReadDto? ProcessGetByIdResult(Contact? entity)
    {
        if (entity == null)
        {
            return null;
        }

        return _mapper.MapToDto(entity);
    }

    public ContactReadDto Create(ContactCreateDto dto)
    {
        _validationFactory.ValidateAndThrow(dto);
        Contact entity = _mapper.MapToEntity(dto);
            
        _repository.Add(entity);
        _repository.SaveChanges();
            
        return _mapper.MapToDto(entity);
    }

    public void Update(ContactUpdateDto dto)
    {
        _validationFactory.ValidateAndThrow(dto);
        Contact? entity = _repository.GetById(dto.Id);
            
        ProcessUpdateEntity(dto, entity);
    }

    private void ProcessUpdateEntity(ContactUpdateDto dto, Contact? entity)
    {
        if (entity != null)
        {
            ExecuteUpdate(dto, entity);
        }
    }

    private void ExecuteUpdate(ContactUpdateDto dto, Contact entity)
    {
        _mapper.ApplyUpdate(dto, entity);
        _repository.Update(entity);
        _repository.SaveChanges();
    }

    public void Patch(long id, ContactPatchDto dto)
    {
        _validationFactory.ValidateAndThrow(dto);
        Contact? entity = _repository.GetById(id);
            
        ProcessPatchEntity(dto, entity);
    }

    private void ProcessPatchEntity(ContactPatchDto dto, Contact? entity)
    {
        if (entity != null)
        {
            ExecutePatch(dto, entity);
        }
    }

    private void ExecutePatch(ContactPatchDto dto, Contact entity)
    {
        _mapper.ApplyPatch(dto, entity);
        _repository.Update(entity);
        _repository.SaveChanges();
    }

    public void Delete(long id)
    {
        Contact? entity = _repository.GetById(id);
        ProcessDeleteEntity(entity);
    }

    private void ProcessDeleteEntity(Contact? entity)
    {
        if (entity != null)
        {
            ExecuteDelete(entity);
        }
    }

    private void ExecuteDelete(Contact entity)
    {
        _repository.Delete(entity);
        _repository.SaveChanges();
    }
}