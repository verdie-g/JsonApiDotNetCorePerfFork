using System.Collections.Generic;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace app.Models
{
    public sealed class Person : Identifiable
    {
        [Attr]
        public string FirstName { get; set; }

        [Attr]
        public string LastName { get; set; }

        [HasMany]
        public ISet<TodoItem> AssignedTodoItems { get; set; }
    }
}
