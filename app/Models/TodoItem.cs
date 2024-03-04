using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace App.Models;

[Resource]
public sealed class TodoItem : Identifiable<int>
{
    [Attr] public string Description { get; set; } = default!;

    [Attr] public TodoItemPriority Priority { get; set; }

    [Attr(Capabilities = AttrCapabilities.AllowFilter | AttrCapabilities.AllowSort | AttrCapabilities.AllowView)]
    public DateTimeOffset CreatedAt { get; set; }

    [Attr(PublicName = "modifiedAt",
        Capabilities = AttrCapabilities.AllowFilter | AttrCapabilities.AllowSort | AttrCapabilities.AllowView)]
    public DateTimeOffset? LastModifiedAt { get; set; }

    [HasOne] public Person Owner { get; set; } = default!;

    [HasOne] public Person Assignee { get; set; } = default!;

    [HasMany] public ISet<Tag> Tags { get; set; } = default!;
}