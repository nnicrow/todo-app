namespace TODOApp.DataAccess.Models;

public class TODOTask : BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedOn { get; set; }
    
    public DateTime? LeadTime { get; set; }
    public bool IsComplete { get; set; } = false;
    public User Owmer { get; set; }
}