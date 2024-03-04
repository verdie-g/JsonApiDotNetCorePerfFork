using System.ComponentModel;
using App.Models;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Middleware;
using JsonApiDotNetCore.Queries.Expressions;
using JsonApiDotNetCore.Resources;
using Microsoft.AspNetCore.Authentication;

namespace App.Definitions;

public sealed class TodoItemDefinition(IResourceGraph resourceGraph, ISystemClock systemClock)
    : JsonApiResourceDefinition<TodoItem, int>(resourceGraph)
{
    public override SortExpression OnApplySort(SortExpression? existingSort)
    {
        return existingSort ?? GetDefaultSortOrder();
    }

    private SortExpression GetDefaultSortOrder()
    {
        return CreateSortExpressionFromLambda([
            (todoItem => todoItem.Priority, ListSortDirection.Descending),
            (todoItem => todoItem.LastModifiedAt, ListSortDirection.Descending)
        ]);
    }

    public override Task OnWritingAsync(TodoItem resource, WriteOperationKind writeOperation,
        CancellationToken cancellationToken)
    {
        if (writeOperation == WriteOperationKind.CreateResource)
            resource.CreatedAt = systemClock.UtcNow;
        else if (writeOperation == WriteOperationKind.UpdateResource) resource.LastModifiedAt = systemClock.UtcNow;

        return Task.CompletedTask;
    }
}