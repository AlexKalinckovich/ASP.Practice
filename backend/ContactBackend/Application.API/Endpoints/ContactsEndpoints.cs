using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Model.Core.DTOs;
using Service.Core;

namespace ContactBackend.Application.API.Endpoints;

public static class ContactEndpoints
{
    public static void MapContactEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/v1/contacts");

        group.MapGet("/", GetAllAsync);
        group.MapGet("/{id:long}", GetByIdAsync);
        group.MapPost("/", CreateAsync);
        group.MapPut("/{id:long}", UpdateAsync);
        group.MapPatch("/{id:long}", PatchAsync);
        group.MapDelete("/{id:long}", DeleteAsync);
    }

    private static async Task<Ok<IEnumerable<ContactReadDto>>> GetAllAsync([FromServices] IContactService service)
    {
        IEnumerable<ContactReadDto> contacts = await service.GetAllAsync();
        
        return TypedResults.Ok(contacts);
    }

    private static async Task<Results<Ok<ContactReadDto>, NotFound>> GetByIdAsync(long id, [FromServices] IContactService service)
    {
        ContactReadDto? contact = await service.GetByIdAsync(id);
        
        return EvaluateGetByIdResult(contact);
    }

    private static Results<Ok<ContactReadDto>, NotFound> EvaluateGetByIdResult(ContactReadDto? contact)
    {
        if (contact == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(contact);
    }

    private static async Task<Results<Created<ContactReadDto>, BadRequest<string>>> CreateAsync([FromBody] ContactCreateDto dto, [FromServices] IContactService service)
    {
        return await ProcessCreateDtoAsync(dto, service);
    }

    private static async Task<Results<Created<ContactReadDto>, BadRequest<string>>> ProcessCreateDtoAsync(ContactCreateDto? dto, IContactService service)
    {
        if (dto == null)
        {
            return TypedResults.BadRequest("Payload cannot be null.");
        }

        return await ExecuteCreateAsync(dto, service);
    }

    private static async Task<Created<ContactReadDto>> ExecuteCreateAsync(ContactCreateDto dto, IContactService service)
    {
        ContactReadDto createdContact = await service.CreateAsync(dto);
        
        return TypedResults.Created($"/api/v1/contacts/{createdContact.Id}", createdContact);
    }

    private static async Task<Results<NoContent, BadRequest<string>>> UpdateAsync(long id, [FromBody] ContactUpdateDto dto, [FromServices] IContactService service)
    {
        return await ProcessUpdateDtoAsync(id, dto, service);
    }

    private static async Task<Results<NoContent, BadRequest<string>>> ProcessUpdateDtoAsync(long id, ContactUpdateDto? dto, IContactService service)
    {
        if (dto == null)
        {
            return TypedResults.BadRequest("Payload cannot be null.");
        }

        return await ExecuteUpdateAsync(id, dto, service);
    }

    private static async Task<NoContent> ExecuteUpdateAsync(long id, ContactUpdateDto dto, IContactService service)
    {
        dto.Id = id;
        await service.UpdateAsync(dto);
        
        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, BadRequest<string>>> PatchAsync(long id, [FromBody] ContactPatchDto dto, [FromServices] IContactService service)
    {
        return await ProcessPatchDtoAsync(id, dto, service);
    }

    private static async Task<Results<NoContent, BadRequest<string>>> ProcessPatchDtoAsync(long id, ContactPatchDto? dto, IContactService service)
    {
        if (dto == null)
        {
            return TypedResults.BadRequest("Payload cannot be null.");
        }

        return await ExecutePatchAsync(id, dto, service);
    }

    private static async Task<NoContent> ExecutePatchAsync(long id, ContactPatchDto dto, IContactService service)
    {
        await service.PatchAsync(id, dto);
        
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteAsync(long id, [FromServices] IContactService service)
    {
        await service.DeleteAsync(id);
        
        return TypedResults.NoContent();
    }
}