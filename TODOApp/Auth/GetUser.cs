using System.Security.Claims;
using TODOApp.DataAccess;
using TODOApp.DataAccess.Models;

namespace TODOApp.Auth;

public class GetUser
{
    public static User? ByClaimsPrincipal(TodoappContext todoappContext, ClaimsPrincipal claimsPrincipal)
    {
        var userName = claimsPrincipal.Identities.FirstOrDefault()?.Name;
        if (userName == null) return null;
        var user = todoappContext.Users?.FirstOrDefault(user => user.Username == userName);
        return user;
    }
}