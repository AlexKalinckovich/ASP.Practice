using Model.Core.Models;

namespace Data.Core;

public class ContactRepository : IContactRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ContactRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Contact> GetAll()
    {
        return _dbContext.Contacts.ToList();
    }

    public Contact? GetById(long id)
    {
        return _dbContext.Contacts.Find(id);
    }

    public void Add(Contact contact)
    {
        _dbContext.Contacts.Add(contact);
    }

    public void Update(Contact contact)
    {
        _dbContext.Contacts.Update(contact);
    }

    public void Delete(Contact contact)
    {
        _dbContext.Contacts.Remove(contact);
    }

    public bool Exists(long id)
    {
        return _dbContext.Contacts.Any(contact => contact.Id == id);
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}