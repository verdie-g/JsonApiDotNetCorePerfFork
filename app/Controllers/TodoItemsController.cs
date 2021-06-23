using app.Models;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;

namespace app.Controllers
{
    public sealed class TodoItemsController : JsonApiController<TodoItem>
    {
        public TodoItemsController(IJsonApiOptions options, ILoggerFactory loggerFactory, IResourceService<TodoItem> resourceService)
            : base(options, loggerFactory, resourceService)
        {
        }
    }
}
