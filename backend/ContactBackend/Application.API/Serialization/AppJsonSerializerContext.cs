using System.Text.Json.Serialization;
using Model.Core.DTOs;

namespace ContactBackend.Application.API.Serialization;

[JsonSerializable(typeof(IEnumerable<ContactReadDto>))]
[JsonSerializable(typeof(ContactReadDto))]
[JsonSerializable(typeof(ContactCreateDto))]
[JsonSerializable(typeof(ContactUpdateDto))]
[JsonSerializable(typeof(ContactPatchDto))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}