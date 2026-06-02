using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Model.Core.DTOs;

namespace ContactBackend.Application.API.Serialization;

[JsonSerializable(typeof(IEnumerable<ContactReadDto>))]
[JsonSerializable(typeof(ContactReadDto))]
[JsonSerializable(typeof(ContactCreateDto))]
[JsonSerializable(typeof(ContactUpdateDto))]
[JsonSerializable(typeof(ContactPatchDto))]
[JsonSerializable(typeof(ValidationProblemDetails))]
[JsonSerializable(typeof(ProblemDetails))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}