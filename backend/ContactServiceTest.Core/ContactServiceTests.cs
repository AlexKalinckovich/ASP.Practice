using Data.Core;
using FluentAssertions;
using Model.Core.DTOs;
using Model.Core.Mappers;
using Model.Core.Models;
using Moq;
using Service.Core;
using Validation.Core;
using Xunit;

namespace ContactServiceTest.Core;

public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _contactRepositoryMock;
    private readonly Mock<IContactMapper> _contactMapperMock;
    private readonly Mock<IValidationFactory> _validationFactoryMock;
    private readonly ContactService _contactService;

    public ContactServiceTests()
    {
        _contactRepositoryMock = InitializeContactRepositoryMock();
        _contactMapperMock = InitializeContactMapperMock();
        _validationFactoryMock = InitializeValidationFactoryMock();
        _contactService = InitializeContactService();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnContactReadDto_WhenValidationSucceeds()
    {
        ContactCreateDto createDto = BuildValidContactCreateDto();
        Contact mappedEntity = BuildMappedContactEntity();
        Contact savedEntity = BuildExistingContactEntity();
        ContactReadDto expectedDto = BuildExpectedContactReadDto();

        ConfigureValidationFactoryToSucceed(createDto);
        ConfigureMapperToMapToEntity(createDto, mappedEntity);
        ConfigureRepositoryToSaveEntityAsync(mappedEntity, savedEntity);
        ConfigureMapperToMapToReadDto(mappedEntity, expectedDto);

        ContactReadDto result = await ExecuteCreateAsync(createDto);

        AssertResultIsExpectedDto(result, expectedDto);
        VerifyRepositoryAddAsyncWasCalled(mappedEntity);
        VerifyRepositorySaveChangesAsyncWasCalled();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAndSaveContact_WhenValidationSucceedsAndContactExists()
    {
        ContactUpdateDto updateDto = BuildValidContactUpdateDto();
        Contact existingEntity = BuildExistingContactEntity();

        ConfigureValidationFactoryToSucceed(updateDto);
        ConfigureRepositoryToReturnEntityByIdAsync(updateDto.Id, existingEntity);

        await ExecuteUpdateAsync(updateDto);

        VerifyMapperApplyUpdateWasCalled(updateDto, existingEntity);
        VerifyRepositoryUpdateWasCalled(existingEntity);
        VerifyRepositorySaveChangesAsyncWasCalled();
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateOrSave_WhenContactDoesNotExist()
    {
        ContactUpdateDto updateDto = BuildValidContactUpdateDto();

        ConfigureValidationFactoryToSucceed(updateDto);
        ConfigureRepositoryToReturnNullByIdAsync(updateDto.Id);

        await ExecuteUpdateAsync(updateDto);

        VerifyMapperApplyUpdateWasNeverCalled(updateDto);
        VerifyRepositoryUpdateWasNeverCalled();
        VerifyRepositorySaveChangesAsyncWasNeverCalled();
    }

    [Fact]
    public async Task PatchAsync_ShouldPatchAndSaveContact_WhenValidationSucceedsAndContactExists()
    {
        long contactId = 1;
        ContactPatchDto patchDto = BuildValidContactPatchDto();
        Contact existingEntity = BuildExistingContactEntity();

        ConfigureValidationFactoryToSucceed(patchDto);
        ConfigureRepositoryToReturnEntityByIdAsync(contactId, existingEntity);

        await ExecutePatchAsync(contactId, patchDto);

        VerifyMapperApplyPatchWasCalled(patchDto, existingEntity);
        VerifyRepositoryUpdateWasCalled(existingEntity);
        VerifyRepositorySaveChangesAsyncWasCalled();
    }

    [Fact]
    public async Task PatchAsync_ShouldNotPatchOrSave_WhenContactDoesNotExist()
    {
        long contactId = 1;
        ContactPatchDto patchDto = BuildValidContactPatchDto();

        ConfigureValidationFactoryToSucceed(patchDto);
        ConfigureRepositoryToReturnNullByIdAsync(contactId);

        await ExecutePatchAsync(contactId, patchDto);

        VerifyMapperApplyPatchWasNeverCalled(patchDto);
        VerifyRepositoryUpdateWasNeverCalled();
        VerifyRepositorySaveChangesAsyncWasNeverCalled();
    }

    private Mock<IContactRepository> InitializeContactRepositoryMock()
    {
        return new Mock<IContactRepository>();
    }

    private Mock<IContactMapper> InitializeContactMapperMock()
    {
        return new Mock<IContactMapper>();
    }

    private Mock<IValidationFactory> InitializeValidationFactoryMock()
    {
        return new Mock<IValidationFactory>();
    }

    private ContactService InitializeContactService()
    {
        return new ContactService(
            _contactRepositoryMock.Object,
            _contactMapperMock.Object,
            _validationFactoryMock.Object
        );
    }

    private ContactCreateDto BuildValidContactCreateDto()
    {
        return new ContactCreateDto
        {
            Name = "Jane Doe",
            MobilePhone = "+19876543210",
            JobTitle = "Software Engineer",
            BirthDate = new DateTime(1990, 5, 15)
        };
    }

    private ContactUpdateDto BuildValidContactUpdateDto()
    {
        return new ContactUpdateDto
        {
            Id = 1,
            Name = "Jane Doe Updated",
            MobilePhone = "+10000000000",
            JobTitle = "Senior Software Engineer",
            BirthDate = new DateTime(1990, 5, 15)
        };
    }

    private ContactPatchDto BuildValidContactPatchDto()
    {
        return new ContactPatchDto
        {
            JobTitle = "Lead Software Engineer"
        };
    }

    private Contact BuildMappedContactEntity()
    {
        return new Contact
        {
            Name = "Jane Doe",
            MobilePhone = "+19876543210",
            JobTitle = "Software Engineer",
            BirthDate = new DateTime(1990, 5, 15)
        };
    }

    private Contact BuildExistingContactEntity()
    {
        return new Contact
        {
            Id = 1,
            Name = "Jane Doe",
            MobilePhone = "+19876543210",
            JobTitle = "Software Engineer",
            BirthDate = new DateTime(1990, 5, 15)
        };
    }

    private ContactReadDto BuildExpectedContactReadDto()
    {
        return new ContactReadDto
        {
            Id = 1,
            Name = "Jane Doe",
            MobilePhone = "+19876543210",
            BirthDate = new DateTime(1990, 5, 15)
        };
    }

    private void ConfigureValidationFactoryToSucceed<T>(T dto)
    {
        _validationFactoryMock.Setup(factory => factory.ValidateAndThrow(dto));
    }

    private void ConfigureMapperToMapToEntity(ContactCreateDto dto, Contact entity)
    {
        _contactMapperMock.Setup(mapper => mapper.MapToEntity(dto)).Returns(entity);
    }

    private void ConfigureRepositoryToSaveEntityAsync(Contact entity, Contact savedEntity)
    {
        _contactRepositoryMock.Setup(repository => repository.AddAsync(entity)).ReturnsAsync(savedEntity);
        _contactRepositoryMock.Setup(repository => repository.SaveChangesAsync()).Returns(Task.CompletedTask);
    }

    private void ConfigureRepositoryToReturnEntityByIdAsync(long id, Contact entity)
    {
        _contactRepositoryMock.Setup(repository => repository.GetByIdAsync(id)).ReturnsAsync(entity);
    }

    private void ConfigureRepositoryToReturnNullByIdAsync(long id)
    {
        _contactRepositoryMock.Setup(repository => repository.GetByIdAsync(id)).ReturnsAsync((Contact?)null);
    }

    private void ConfigureMapperToMapToReadDto(Contact entity, ContactReadDto dto)
    {
        _contactMapperMock.Setup(mapper => mapper.MapToDto(It.IsAny<Contact>())).Returns(dto);
    }

    private async Task<ContactReadDto> ExecuteCreateAsync(ContactCreateDto dto)
    {
        return await _contactService.CreateAsync(dto);
    }

    private async Task ExecuteUpdateAsync(ContactUpdateDto dto)
    {
        await _contactService.UpdateAsync(dto);
    }

    private async Task ExecutePatchAsync(long id, ContactPatchDto dto)
    {
        await _contactService.PatchAsync(id, dto);
    }

    private void AssertResultIsExpectedDto(ContactReadDto result, ContactReadDto expected)
    {
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected);
    }

    private void VerifyRepositoryAddAsyncWasCalled(Contact entity)
    {
        _contactRepositoryMock.Verify(repository => repository.AddAsync(entity), Times.Once);
    }

    private void VerifyMapperApplyUpdateWasCalled(ContactUpdateDto dto, Contact entity)
    {
        _contactMapperMock.Verify(mapper => mapper.ApplyUpdate(dto, entity), Times.Once);
    }

    private void VerifyMapperApplyUpdateWasNeverCalled(ContactUpdateDto dto)
    {
        _contactMapperMock.Verify(mapper => mapper.ApplyUpdate(dto, It.IsAny<Contact>()), Times.Never);
    }

    private void VerifyMapperApplyPatchWasCalled(ContactPatchDto dto, Contact entity)
    {
        _contactMapperMock.Verify(mapper => mapper.ApplyPatch(dto, entity), Times.Once);
    }

    private void VerifyMapperApplyPatchWasNeverCalled(ContactPatchDto dto)
    {
        _contactMapperMock.Verify(mapper => mapper.ApplyPatch(dto, It.IsAny<Contact>()), Times.Never);
    }

    private void VerifyRepositoryUpdateWasCalled(Contact entity)
    {
        _contactRepositoryMock.Verify(repository => repository.Update(entity), Times.Once);
    }

    private void VerifyRepositoryUpdateWasNeverCalled()
    {
        _contactRepositoryMock.Verify(repository => repository.Update(It.IsAny<Contact>()), Times.Never);
    }

    private void VerifyRepositorySaveChangesAsyncWasCalled()
    {
        _contactRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Once);
    }

    private void VerifyRepositorySaveChangesAsyncWasNeverCalled()
    {
        _contactRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Never);
    }
}