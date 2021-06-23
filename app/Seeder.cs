using System;
using System.Collections.Generic;
using System.Linq;
using app.Data;
using app.Models;

namespace app
{
    internal static class Seeder
    {
        public static void EnsureSampleData(AppDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            List<Tag> tags = CreateTags().ToList();
            List<Person> people = CreatePeople().ToList();
            List<TodoItem> todoItems = CreateTodoItems(tags, people).ToList();

            dbContext.Tags.AddRange(tags);
            dbContext.People.AddRange(people);
            dbContext.TodoItems.AddRange(todoItems);

            dbContext.SaveChanges();
        }

        private static IEnumerable<Tag> CreateTags()
        {
            foreach (string tagName in new[]
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
            foreach ((string firstName, string lastName) in new[]
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
            {
                yield return new Person
                {
                    FirstName = firstName,
                    LastName = lastName
                };
            }
        }

        private static IEnumerable<TodoItem> CreateTodoItems(IList<Tag> tags, IList<Person> people)
        {
            var priorityIterator = new LoopingCollectionIterator<TodoItemPriority>((TodoItemPriority[])Enum.GetValues(typeof(TodoItemPriority)));
            var ownerIterator = new LoopingCollectionIterator<Person>(people);
            var assigneeIterator = new LoopingCollectionIterator<Person>(people, 2);
            var tagIterator = new LoopingCollectionIterator<Tag>(tags);

            var creationTime = new DateTimeOffset(2000, 1, 1, 1, 1, 1, 1, TimeSpan.FromHours(5));
            var lastModifiedOffset = new TimeSpan(2, 2, 2, 2, 2);

            for (int todoItemIndex = 1; todoItemIndex <= 1000; todoItemIndex++)
            {
                yield return new TodoItem
                {
                    Description = $"Activity {todoItemIndex:D5}",
                    Priority = priorityIterator.GetNext(),
                    CreatedAt = creationTime,
                    LastModifiedAt = creationTime + lastModifiedOffset,
                    Owner = ownerIterator.GetNext(),
                    Assignee = assigneeIterator.GetNext(),
                    TodoItemTags = new HashSet<TodoItemTag>
                    {
                        new TodoItemTag
                        {
                            Tag = tagIterator.GetNext()
                        },
                        new TodoItemTag
                        {
                            Tag = tagIterator.GetNext()
                        },
                        new TodoItemTag
                        {
                            Tag = tagIterator.GetNext()
                        }
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
                if (offset >= collection.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(offset));
                }

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
}
