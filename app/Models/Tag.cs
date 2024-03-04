using System.ComponentModel.DataAnnotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace App.Models;

[Resource]
public sealed class Tag : Identifiable<int>
{
    [Required] [MinLength(1)] [Attr] public string Name { get; set; } = default!;
}