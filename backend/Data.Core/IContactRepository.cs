using Model.Core.Models;

namespace Data.Core;

public interface IContactRepository
{
    IEnumerable<Contact> GetAll();
    Contact? GetById(long id);
    void Add(Contact contact);
    void Update(Contact contact);
    void Delete(Contact contact);
    bool Exists(long id);
    void SaveChanges();
}