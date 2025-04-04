using RangoAgil.API.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])
    );

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("rango/{nome}", async (RangoDbContext rangoDbContext, string nome) =>{

    return await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Nome == nome);

});

app.MapGet("/rango/{id:int}", async(RangoDbContext rangoDbContext, int id) => {

    return await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == id);

});

app.MapGet("/rangos", async (RangoDbContext rangoDbContext) =>
{
    return await rangoDbContext.Rangos.ToListAsync();

});

app.Run();
