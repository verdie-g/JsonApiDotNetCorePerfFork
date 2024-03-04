﻿using App.Data;
using App.Models;

namespace App;

internal static class Seeder
{
    public static async Task EnsureSampleDataAsync(AppDbContext dbContext)
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var tags = CreateTags().ToArray();
        var people = CreatePeople().ToArray();
        var todoItems = CreateTodoItems(tags, people).ToArray();

        dbContext.Tags.AddRange(tags);
        dbContext.People.AddRange(people);
        dbContext.TodoItems.AddRange(todoItems);

        await dbContext.SaveChangesAsync();
    }

    private static IEnumerable<Tag> CreateTags()
    {
        foreach (var tagName in new[]
                 {
                     "Work",
                     "Home",
                     "Friends",
                     "Family",
                     "Holidays",
                     "Hobbies",
                     "Mail",
                     "Computer",
                     "Internet",
                     "Books",
                     "Health",
                     "Food",
                     "Movies"
                 })
        {
            yield return new Tag
            {
                Name = tagName
            };
        }
    }

    private static IEnumerable<Person> CreatePeople()
    {
        foreach (var (firstName, lastName) in new[]
                 {
                     ("Moreen", "Cooke"),
                     ("Brock", "Garnett"),
                     ("Seanna", "Eliot"),
                     ("Rexanne", "Harley"),
                     ("Barrett", "Hume"),
                     ("Bentley", "Emerson"),
                     ("Ennis", "Slater"),
                     ("Irvin", "Sangster"),
                     ("Gil", "Marlowe"),
                     ("Johnathon", "Joyner"),
                     ("Talbot", "Elmer"),
                     ("Debbi", "Stevenson"),
                     ("Nigel", "Harlan"),
                     ("Wilda", "Jeanes"),
                     ("Royal", "Harden"),
                     ("Dotty", "Thompsett"),
                     ("Glenda", "Clay"),
                     ("Githa", "Harmon"),
                     ("Earnest", "Foster"),
                     ("Karissa", "Hubbard")
                 })
            yield return new Person
            {
                FirstName = firstName,
                LastName = lastName
            };
    }

    private static IEnumerable<TodoItem> CreateTodoItems(IList<Tag> tags, IList<Person> people)
    {
        var priorityIterator =
            new LoopingCollectionIterator<TodoItemPriority>(
                (TodoItemPriority[])Enum.GetValues(typeof(TodoItemPriority)));
        var ownerIterator = new LoopingCollectionIterator<Person>(people);
        var assigneeIterator = new LoopingCollectionIterator<Person>(people, 2);
        var tagIterator = new LoopingCollectionIterator<Tag>(tags);

        var creationTime = new DateTimeOffset(2000, 1, 1, 1, 1, 1, 1, TimeSpan.Zero);
        var lastModifiedOffset = new TimeSpan(2, 2, 2, 2, 2);

        for (var todoItemIndex = 1; todoItemIndex <= 1000; todoItemIndex++)
        {
            yield return new TodoItem
            {
                Description = $"Activity {todoItemIndex:D5}",
                Priority = priorityIterator.GetNext(),
                CreatedAt = creationTime,
                LastModifiedAt = creationTime + lastModifiedOffset,
                Owner = ownerIterator.GetNext(),
                Assignee = assigneeIterator.GetNext(),
                Tags = new HashSet<Tag>
                {
                    tagIterator.GetNext(),
                    tagIterator.GetNext(),
                    tagIterator.GetNext()
                }
            };

            creationTime += TimeSpan.FromDays(1);
        }
    }

    private sealed class LoopingCollectionIterator<T>
    {
        private readonly IList<T> _collection;
        private int _index;

        public LoopingCollectionIterator(IList<T> collection, int offset = 0)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(offset, collection.Count);

            _collection = collection;
            _index = offset - 1;
        }

        public T GetNext()
        {
            _index = (_index + 1) % _collection.Count;
            return _collection[_index];
        }
    }
}