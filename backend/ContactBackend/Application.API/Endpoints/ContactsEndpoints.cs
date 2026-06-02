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

        group.MapGet("/", GetAll);
        group.MapGet("/{id:long}", GetById);
        group.MapPost("/", Create);
        group.MapPut("/{id:long}", Update);
        group.MapPatch("/{id:long}", Patch);
        group.MapDelete("/{id:long}", Delete);
    }

    private static Ok<IEnumerable<ContactReadDto>> GetAll([FromServices] IContactService service)
    {
        IEnumerable<ContactReadDto> contacts = service.GetAll();
        return TypedResults.Ok(contacts);
    }

    private static Results<Ok<ContactReadDto>, NotFound> GetById(long id, [FromServices] IContactService service)
    {
        ContactReadDto? contact = service.GetById(id);
        return EvaluateGetById(contact);
    }

    private static Results<Ok<ContactReadDto>, NotFound> EvaluateGetById(ContactReadDto? contact)
    {
        if (contact == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(contact);
    }

    private static Results<Created<ContactReadDto>, BadRequest<string>> Create([FromBody] ContactCreateDto dto, [FromServices] IContactService service)
    {
        return ProcessCreateDto(dto, service);
    }

    private static Results<Created<ContactReadDto>, BadRequest<string>> ProcessCreateDto(ContactCreateDto? dto, IContactService service)
    {
        if (dto == null)
        {
            return TypedResults.BadRequest("Payload cannot be null.");
        }

        return ExecuteCreate(dto, service);
    }

    private static Created<ContactReadDto> ExecuteCreate(ContactCreateDto dto, IContactService service)
    {
        ContactReadDto createdContact = service.Create(dto);
        return TypedResults.Created($"/api/v1/contacts/{createdContact.Id}", createdContact);
    }

    private static Results<NoContent, BadRequest<string>> Update(long id, [FromBody] ContactUpdateDto dto, [FromServices] IContactService service)
    {
        return ProcessUpdateDto(id, dto, service);
    }

    private static Results<NoContent, BadRequest<string>> ProcessUpdateDto(long id, ContactUpdateDto? dto, IContactService service)
    {
        if (dto == null)
        {
            return TypedResults.BadRequest("Payload cannot be null.");
        }

        return ExecuteUpdate(id, dto, service);
    }

    private static NoContent ExecuteUpdate(long id, ContactUpdateDto dto, IContactService service)
    {
        dto.Id = id;
        service.Update(dto);
        return TypedResults.NoContent();
    }

    private static Results<NoContent, BadRequest<string>> Patch(long id, [FromBody] ContactPatchDto dto, [FromServices] IContactService service)
    {
        return ProcessPatchDto(id, dto, service);
    }

    private static Results<NoContent, BadRequest<string>> ProcessPatchDto(long id, ContactPatchDto? dto, IContactService service)
    {
        if (dto == null)
        {
            return TypedResults.BadRequest("Payload cannot be null.");
        }

        return ExecutePatch(id, dto, service);
    }

    private static NoContent ExecutePatch(long id, ContactPatchDto dto, IContactService service)
    {
        service.Patch(id, dto);
        return TypedResults.NoContent();
    }

    private static NoContent Delete(long id, [FromServices] IContactService service)
    {
        service.Delete(id);
        return TypedResults.NoContent();
    }
}