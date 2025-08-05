using Snipper_Snippets_API.Services;
using Snipper_Snippets_API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SnippetService>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<EncryptionService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseMiddleware<BasicAuthMiddleware>();

app.MapControllers();

app.Run();
