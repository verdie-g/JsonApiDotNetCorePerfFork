using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; } = default!;
    public DbSet<Person> People { get; set; } = default!;
    public DbSet<Tag> Tags { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // When deleting a person, un-assign him/her from existing todo items.
        builder.Entity<Person>()
            .HasMany(person => person.AssignedTodoItems)
            .WithOne(todoItem => todoItem.Assignee)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        // When deleting a person, the todo items he/she owns are deleted too.
        builder.Entity<TodoItem>()
            .HasOne(todoItem => todoItem.Owner)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}