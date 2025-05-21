using LibraryManager.Data.BD;
using LibraryManager.Shared.Data.DB;
using LibraryManager_API.Endpoints;
using LibraryManager_Console;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Adiciona configuração para ignorar referências cíclicas
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<LibraryManagerContext>();
builder.Services.AddTransient<DAL<Book>>();
builder.Services.AddScoped<DAL<Genre>>();
builder.Services.AddScoped<DAL<Reader>>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.AddEndPointsBook();
app.AddEndPointsGenre();
app.AddEndPointsReader();


app.UseSwagger();
app.UseSwaggerUI();

//app.MapGet("/", () =>
//{
//    var dal = new DAL<Book>();
//    return dal.Read();
//});

app.Run();
