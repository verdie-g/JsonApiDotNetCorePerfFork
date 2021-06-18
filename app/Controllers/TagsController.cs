using app.Models;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;

namespace app.Controllers
{
    public sealed class TagsController : JsonApiController<Tag>
    {
        public TagsController(IJsonApiOptions options, ILoggerFactory loggerFactory, IResourceService<Tag> resourceService)
            : base(options, loggerFactory, resourceService)
        {
        }
    }
}
