using LibraryManager.Data.BD;
using LibraryManager.Shared.Data.DB;
using LibraryManager.Shared.Data.Models;
using LibraryManager_API.Endpoints;
using LibraryManager_Console;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Adiciona configuração para ignorar referências cíclicas
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<LibraryManagerContext>();
builder.Services.AddTransient<DAL<Book>>();
builder.Services.AddScoped<DAL<Genre>>();
builder.Services.AddScoped<DAL<Reader>>();
builder.Services.AddTransient<DAL<Publisher>>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentityApiEndpoints<AccessUser>().AddEntityFrameworkStores<LibraryManagerContext>();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthorization();

app.MapGroup("auth").MapIdentityApi<AccessUser>().WithTags("Authorization");

app.MapPost("auth/logout", async (HttpContext httpContext) => {
    var signInManager = httpContext.RequestServices.GetRequiredService<SignInManager<AccessUser>>();
    await signInManager.SignOutAsync();
    return Results.Ok();
})
.RequireAuthorization()
.WithTags("Authorization");

app.AddEndPointsBook();
app.AddEndPointsGenre();
app.AddEndPointsReader();
app.AddEndPointsPublisher();

app.UseSwagger();
app.UseSwaggerUI();

//app.MapGet("/", () =>
//{
//    var dal = new DAL<Book>();
//    return dal.Read();
//});

app.Run();
