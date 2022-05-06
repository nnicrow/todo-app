namespace TODOApp.GraphQL.Output;

public class AuthOutput
{
    public bool Status { get; set; } = false;
    public string? Token { get; set; } = null;
    public string? Error { get; set; } = null;
}