using RangoAgil.API.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])
    );

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("rango/{nome}", (RangoDbContext rangoDbContext, string nome) =>{

    return rangoDbContext.Rangos.FirstOrDefault(x => x.Nome == nome);

});

app.MapGet("/rango/{id:int}", (RangoDbContext rangoDbContext, int id) => {

    return rangoDbContext.Rangos.First(x => x.Id == id);

});

app.MapGet("/rangos", (RangoDbContext rangoDbContext) =>
{
    return rangoDbContext.Rangos;

});

app.Run();
