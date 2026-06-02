using Model.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Core;

public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<Contact?> GetByIdAsync(long id);
    Task<IEnumerable<Contact>> GetAllReadOnlyAsync();
    Task<Contact?> GetByIdReadOnlyAsync(long id);
    Task<Contact> AddAsync(Contact contact);
    void Update(Contact contact);
    void Delete(Contact contact);
    Task<bool> ExistsAsync(long id);
    Task SaveChangesAsync();
}