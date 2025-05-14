using LibraryManager.Data.BD;
using LibraryManager_Console;
using System.Text.Json.Serialization; // necess�rio para ReferenceHandler

var builder = WebApplication.CreateBuilder(args);

// Adiciona configura��o para ignorar refer�ncias c�clicas
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.MapGet("/", () =>
{
    var dal = new DAL<Book>();
    return dal.Read();
});

app.Run();
