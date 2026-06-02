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

    public async Task<IEnumerable<ContactReadDto>> GetAllAsync()
    {
        IEnumerable<Contact> entities = await _repository.GetAllReadOnlyAsync();
        
        return TransformEntitiesToDtos(entities);
    }

    private IEnumerable<ContactReadDto> TransformEntitiesToDtos(IEnumerable<Contact> entities)
    {
        return entities.Select(MapEntityToDto).ToList();
    }

    private ContactReadDto MapEntityToDto(Contact entity)
    {
        return _mapper.MapToDto(entity);
    }

    public async Task<ContactReadDto?> GetByIdAsync(long id)
    {
        Contact? entity = await _repository.GetByIdReadOnlyAsync(id);
        
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

    public async Task<ContactReadDto> CreateAsync(ContactCreateDto dto)
    {
        _validationFactory.ValidateAndThrow(dto);
        Contact entity = _mapper.MapToEntity(dto);
            
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
            
        return _mapper.MapToDto(entity);
    }

    public async Task UpdateAsync(ContactUpdateDto dto)
    {
        _validationFactory.ValidateAndThrow(dto);
        Contact? entity = await _repository.GetByIdAsync(dto.Id);
            
        await ProcessUpdateEntityAsync(dto, entity);
    }

    private async Task ProcessUpdateEntityAsync(ContactUpdateDto dto, Contact? entity)
    {
        if (entity != null)
        {
            await ExecuteUpdateAsync(dto, entity);
        }
    }

    private async Task ExecuteUpdateAsync(ContactUpdateDto dto, Contact entity)
    {
        _mapper.ApplyUpdate(dto, entity);
        _repository.Update(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task PatchAsync(long id, ContactPatchDto dto)
    {
        _validationFactory.ValidateAndThrow(dto);
        Contact? entity = await _repository.GetByIdAsync(id);
            
        await ProcessPatchEntityAsync(dto, entity);
    }

    private async Task ProcessPatchEntityAsync(ContactPatchDto dto, Contact? entity)
    {
        if (entity != null)
        {
            await ExecutePatchAsync(dto, entity);
        }
    }

    private async Task ExecutePatchAsync(ContactPatchDto dto, Contact entity)
    {
        _mapper.ApplyPatch(dto, entity);
        _repository.Update(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        Contact? entity = await _repository.GetByIdAsync(id);
        
        await ProcessDeleteEntityAsync(entity);
    }

    private async Task ProcessDeleteEntityAsync(Contact? entity)
    {
        if (entity != null)
        {
            await ExecuteDeleteAsync(entity);
        }
    }

    private async Task ExecuteDeleteAsync(Contact entity)
    {
        _repository.Delete(entity);
        await _repository.SaveChangesAsync();
    }
}