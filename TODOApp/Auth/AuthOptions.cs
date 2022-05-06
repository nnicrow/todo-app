using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TODOApp.Auth;

public class AuthOptions
{
    public const string ISSUER = "Nnicrow"; 
    public const string AUDIENCE = "TODOApp";
    const string KEY = "IrdnLtE4dJvIE0rgUFSioM8YvNWWLmfF3HxVTyR7Kbj8r4QHL2yQhfObg5pBCACD";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}