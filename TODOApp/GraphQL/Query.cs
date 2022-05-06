using System.Security.Claims;
using HotChocolate.AspNetCore.Authorization;
using TODOApp.Auth;
using TODOApp.DataAccess;
using TODOApp.DataAccess.Models;
using TODOApp.GraphQL.Input;

namespace TODOApp.GraphQL;

public class Query
{
    [UseDbContext(typeof(TodoappContext))]
    [Authorize]
    public IQueryable<TODOTask>? GetTaskList(TodoappContext todoappContext,
        ClaimsPrincipal claimsPrincipal)
    {
        var user = GetUser.ByClaimsPrincipal(todoappContext, claimsPrincipal);
        if (user == null) return null;
        var todoTask = todoappContext.TodoTasks?.Where(task => task.Owmer == user);
        return todoTask;
    }

    [UseDbContext(typeof(TodoappContext))]
    public TODOTask? GetTaskById(TodoTaskId todoTaskId, TodoappContext todoappContext, 
        ClaimsPrincipal claimsPrincipal)
    {
        var user = GetUser.ByClaimsPrincipal(todoappContext, claimsPrincipal);
        if (user == null) return null;
        var todoTask = todoappContext.TodoTasks?
            .FirstOrDefault(task => task.ID == todoTaskId.ID && task.Owmer == user);
        return todoTask;
    }
}