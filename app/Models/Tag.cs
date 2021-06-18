using System.ComponentModel.DataAnnotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace app.Models
{
    public sealed class Tag : Identifiable
    {
        [Required]
        [MinLength(1)]
        [Attr]
        public string Name { get; set; }
    }
}
