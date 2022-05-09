using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;
using TODOApp.Auth;
using TODOApp.DataAccess;
using TODOApp.GraphQL;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<TodoappContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")!));

GraphQlStartup.ConfigureServices(builder.Services);
Authentication.ConfigureServices(builder.Services);
builder.Services.AddAuthorization();
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\temp-keys\"))
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("http://localhost:8080/",
                    "http://host.docker.internal:8080/")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);
app.UseEndpoints(endpoints =>
    {
        endpoints.MapGraphQL()
            .RequireCors(myAllowSpecificOrigins);
    });
app.Run();