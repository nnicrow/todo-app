using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using TODOApp.Auth;
using TODOApp.DataAccess;
using TODOApp.DataAccess.Models;
using TODOApp.GraphQL.Input;
using TODOApp.GraphQL.Output;

namespace TODOApp.GraphQL;

public class Mutation
{
    [UseDbContext(typeof(TodoappContext))]
    [Authorize]
    public async Task<TODOTask?> AddTodoTask(TodoTaskInput todoTaskInput,
        TodoappContext todoappContext, ClaimsPrincipal claimsPrincipal)
    {
        var user = GetUser.ByClaimsPrincipal(todoappContext, claimsPrincipal);
        if (user == null) return null;
        var todoTask = new TODOTask
        {
            Title = todoTaskInput.Title,
            Description = todoTaskInput.Description,
            CreatedOn = DateTime.UtcNow,
            LeadTime = todoTaskInput.LeadTime,
            Owmer = user
        };
        await todoappContext.TodoTasks!.AddAsync(todoTask);
        await todoappContext.SaveChangesAsync();
        return todoTask;
    }

    [UseDbContext(typeof(TodoappContext))]
    public async Task<bool?> DeleteTodoTask(TodoTaskId todoTaskId, 
        TodoappContext todoappContext, ClaimsPrincipal claimsPrincipal)
    {
        var user = GetUser.ByClaimsPrincipal(todoappContext, claimsPrincipal);
        if (user == null) return null;
        
        if (todoappContext.TodoTasks == null) return false;
        var todoTask = todoappContext.TodoTasks
            .FirstOrDefault(task => task.ID == todoTaskId.ID && task.Owmer == user);
        todoappContext.Remove(todoTask);
        await todoappContext.SaveChangesAsync();
        return true;

    }

    [UseDbContext(typeof(TodoappContext))]
    public async Task<TODOTask?> ToggleIsCompleteTaskById(TodoTaskId todoTaskId,
        TodoappContext todoappContext, ClaimsPrincipal claimsPrincipal)
    {
        var user = GetUser.ByClaimsPrincipal(todoappContext, claimsPrincipal);
        if (user == null) return null;
        
        var todoTask = todoappContext.TodoTasks?.FirstOrDefault(task => task.ID == todoTaskId.ID && task.Owmer == user);
        if (todoTask == null) return null;
        todoTask.IsComplete = !todoTask.IsComplete;
        todoappContext.TodoTasks?.Update(todoTask);
        await todoappContext.SaveChangesAsync();
        return todoTask;
    }
    
    [UseDbContext(typeof(TodoappContext))]
    public async Task<TODOTask?> UpdateTaskById(TodoTaskId todoTaskId, TodoTaskInput todoTaskInput,
        TodoappContext todoappContext, ClaimsPrincipal claimsPrincipal)
    {
        var user = GetUser.ByClaimsPrincipal(todoappContext, claimsPrincipal);
        if (user == null) return null;
        
        var todoTask = todoappContext.TodoTasks?.FirstOrDefault(task => task.ID == todoTaskId.ID && task.Owmer == user);
        if (todoTask == null) return null;
        todoTask.Title = todoTaskInput.Title;
        todoTask.Description = todoTaskInput.Description;
        todoTask.LeadTime = todoTaskInput.LeadTime;
        todoappContext.TodoTasks?.Update(todoTask);
        await todoappContext.SaveChangesAsync();
        return todoTask;
    }

    [UseDbContext(typeof(TodoappContext))]
    public async Task<AuthOutput> RegisterUser(UserInput userInput,
        TodoappContext todoappContext)
    {
        if (todoappContext.Users.FirstOrDefault(task => task.Username == userInput.Username) != null) 
            return new AuthOutput { Error = "User with this username already exist" };
        var user = new User
        {
            Username = userInput.Username,
            Password = userInput.Password,
            CreatedOn = DateTime.UtcNow,
        };
        await todoappContext.Users!.AddAsync(user);
        await todoappContext.SaveChangesAsync();
        return new AuthOutput { Token = Jwt.Create(userInput.Username), Status = true };
    }

    [UseDbContext(typeof(TodoappContext))]
    public async Task<AuthOutput> AuthUser(UserInput userInput,
        TodoappContext todoappContext)
    {
        var user = todoappContext.Users.FirstOrDefault(task => task.Username == userInput.Username); 
        if (user == null) return new AuthOutput {Error = "User is not exist"};
        return user.Password != userInput.Password ? new AuthOutput {Error = "Password is not correct"} : new AuthOutput { Token = Jwt.Create(userInput.Username), Status = true };
    }
}