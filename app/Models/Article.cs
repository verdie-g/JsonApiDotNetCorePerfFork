using JsonApiDotNetCore.Models;

namespace app.Models
{
    public class Article : Identifiable
    {
        [Attr("name")]
        public string Name { get; set; }
    }
}