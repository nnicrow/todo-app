namespace TODOApp.GraphQL.Input;

public class TodoTaskInput
{
    public string Title { get; set; }
    public string? Description { get; set; }
    
    public string? LeadTime { get; set; }
}