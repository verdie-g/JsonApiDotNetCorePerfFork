using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace App.Models;

[Resource]
public sealed class Person : Identifiable<int>
{
    [Attr] public string FirstName { get; set; } = default!;

    [Attr] public string LastName { get; set; } = default!;

    [HasMany] public ISet<TodoItem> AssignedTodoItems { get; set; } = default!;
}