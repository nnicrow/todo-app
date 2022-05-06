namespace TODOApp.DataAccess.Models;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime CreatedOn { get; set; }
}