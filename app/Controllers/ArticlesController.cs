using app.Models;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;

namespace app.Controllers
{
    public class ArticlesController : JsonApiController<Article>
    {
        public ArticlesController(IJsonApiOptions options, ILoggerFactory loggerFactory,
            IResourceService<Article, int> resourceService)
            : base(options, loggerFactory, resourceService)
        {
        }
    }
}
