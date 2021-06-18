using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using app.Models;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Middleware;
using JsonApiDotNetCore.Queries.Expressions;
using JsonApiDotNetCore.Resources;
using Microsoft.AspNetCore.Authentication;

namespace app.Definitions
{
    public sealed class TodoItemDefinition : JsonApiResourceDefinition<TodoItem>
    {
        private readonly ISystemClock _systemClock;

        public TodoItemDefinition(IResourceGraph resourceGraph, ISystemClock systemClock)
            : base(resourceGraph)
        {
            _systemClock = systemClock;
        }

        public override SortExpression OnApplySort(SortExpression existingSort)
        {
            return existingSort ?? GetDefaultSortOrder();
        }

        private SortExpression GetDefaultSortOrder()
        {
            return CreateSortExpressionFromLambda(new PropertySortOrder
            {
                (todoItem => todoItem.Priority, ListSortDirection.Descending),
                (todoItem => todoItem.LastModifiedAt, ListSortDirection.Descending)
            });
        }

        public override Task OnWritingAsync(TodoItem resource, OperationKind operationKind, CancellationToken cancellationToken)
        {
            if (operationKind == OperationKind.CreateResource)
            {
                resource.CreatedAt = _systemClock.UtcNow;
            }
            else if (operationKind == OperationKind.UpdateResource)
            {
                resource.LastModifiedAt = _systemClock.UtcNow;
            }

            return Task.CompletedTask;
        }
    }
}
