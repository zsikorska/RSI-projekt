using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogManager.Data;

public class AppDbContext : DbContext
{
    public DbSet<Person>? Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("DataSource = personsdb.db; Cache=Shared");
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Person>().ToTable("Person");
    //}
}