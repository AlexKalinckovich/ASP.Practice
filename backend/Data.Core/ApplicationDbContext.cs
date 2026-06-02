using Microsoft.EntityFrameworkCore;

namespace Data.Core;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : 
    DbContext(options)
{
    public DbSet<Model.Core.Models.Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
    }
}