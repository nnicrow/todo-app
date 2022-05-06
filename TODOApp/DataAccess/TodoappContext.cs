using Microsoft.EntityFrameworkCore;
using TODOApp.DataAccess.Models;

namespace TODOApp.DataAccess;

public class TodoappContext : DbContext
{
    public TodoappContext(DbContextOptions options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        Database.EnsureCreated();
    }
    public DbSet<TODOTask>? TodoTasks { get; set; }
    public DbSet<User>? Users { get; set; }
}