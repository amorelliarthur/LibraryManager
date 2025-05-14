using LibraryManager.Data.BD;
using LibraryManager_Console;
using System.Text.Json.Serialization; // necessário para ReferenceHandler

var builder = WebApplication.CreateBuilder(args);

// Adiciona configuração para ignorar referências cíclicas
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.MapGet("/", () =>
{
    var dal = new DAL<Book>();
    return dal.Read();
});

app.Run();
