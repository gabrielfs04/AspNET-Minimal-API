using RangoAgil.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using RangoAgil.API.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])
    );

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/rangos", async Task<Results<NoContent, Ok<List<Rango>>>>
    (RangoDbContext rangoDbContext,
    [FromQuery(Name = "name")]string? rangoNome) =>{

    var rangosEntity = await rangoDbContext.Rangos.
                                Where(x => rangoNome == null || x.Nome.ToLower().Contains(rangoNome.ToLower()))
                                .ToListAsync();
    if (rangosEntity.Count <= 0 || rangosEntity == null)
        return TypedResults.NoContent();
    else
        return TypedResults.Ok(rangosEntity);

});

app.MapGet("/rangos/{rangoId:int}/ingredientes", async (RangoDbContext rangoDbContext, int rangoId) =>
{
    return await rangoDbContext.Rangos
                               .Include(rango => rango.Ingredientes)
                               .FirstOrDefaultAsync(rango => rango.Id == rangoId);
});

app.MapGet("/rango/{id:int}", async(RangoDbContext rangoDbContext, int id) => {

    return await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == id);

});

app.Run();
