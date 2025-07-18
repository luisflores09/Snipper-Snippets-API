using Snipper_Snippets_API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SnippetService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
