using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model.Core.Models;

namespace Data.Core;

public class ContactRepository : IContactRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ContactRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        return await FetchAllContactsFromDatabaseAsync();
    }

    private async Task<IEnumerable<Contact>> FetchAllContactsFromDatabaseAsync()
    {
        List<Contact> contacts = await _dbContext.Contacts.ToListAsync();
        
        return contacts;
    }

    public async Task<Contact?> GetByIdAsync(long id)
    {
        return await FetchContactByIdFromDatabaseAsync(id);
    }

    private async Task<Contact?> FetchContactByIdFromDatabaseAsync(long id)
    {
        Contact? contact = await _dbContext.Contacts.FindAsync(id);
        
        return contact;
    }

    public async Task<IEnumerable<Contact>> GetAllReadOnlyAsync()
    {
        return await FetchAllContactsAsNoTrackingAsync();
    }

    private async Task<IEnumerable<Contact>> FetchAllContactsAsNoTrackingAsync()
    {
        List<Contact> contacts = await _dbContext.Contacts.AsNoTracking().ToListAsync();
        
        return contacts;
    }

    public async Task<Contact?> GetByIdReadOnlyAsync(long id)
    {
        return await FetchContactByIdAsNoTrackingAsync(id);
    }

    private async Task<Contact?> FetchContactByIdAsNoTrackingAsync(long id)
    {
        Contact? contact = await _dbContext.Contacts.AsNoTracking().FirstOrDefaultAsync(contact => contact.Id == id);
        
        return contact;
    }

    public async Task<Contact> AddAsync(Contact contact)
    {
        return await InsertContactIntoDatabaseAsync(contact);
    }

    private async Task<Contact> InsertContactIntoDatabaseAsync(Contact contact)
    {
        EntityEntry<Contact> entityEntry = await _dbContext.Contacts.AddAsync(contact);
        
        return entityEntry.Entity;
    }

    public void Update(Contact contact)
    {
        UpdateContactInDatabaseContext(contact);
    }

    private void UpdateContactInDatabaseContext(Contact contact)
    {
        _dbContext.Contacts.Update(contact);
    }

    public void Delete(Contact contact)
    {
        RemoveContactFromDatabaseContext(contact);
    }

    private void RemoveContactFromDatabaseContext(Contact contact)
    {
        _dbContext.Contacts.Remove(contact);
    }

    public async Task<bool> ExistsAsync(long id)
    {
        return await CheckIfContactExistsInDatabaseAsync(id);
    }

    private async Task<bool> CheckIfContactExistsInDatabaseAsync(long id)
    {
        bool exists = await _dbContext.Contacts.AnyAsync(contact => contact.Id == id);
        
        return exists;
    }

    public async Task SaveChangesAsync()
    {
        await CommitChangesToDatabaseAsync();
    }

    private async Task CommitChangesToDatabaseAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}