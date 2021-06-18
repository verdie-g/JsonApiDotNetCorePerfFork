using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace app.Models
{
    public class Article : Identifiable
    {
        [Attr]
        public string Name { get; set; }
    }
}
