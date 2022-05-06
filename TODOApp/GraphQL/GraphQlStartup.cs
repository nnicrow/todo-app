using TODOApp.DataAccess;

namespace TODOApp.GraphQL;

public static class GraphQlStartup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .RegisterDbContext<TodoappContext>(DbContextKind.Pooled)
            .AddAuthorization()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddProjections()
            .AddFiltering()
            .AddSorting()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
}