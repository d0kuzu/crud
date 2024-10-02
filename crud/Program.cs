using crud;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder();
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Items API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Items API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/Items", async (ApplicationContext db) => await db.Items.ToListAsync());

app.MapGet("/api/Items/{id:int}", async (int id, ApplicationContext db) =>
{
    Item? Item = await db.Items.FirstOrDefaultAsync(u => u.Id == id);

    if (Item == null) return Results.NotFound(new { message = "Пользователь не найден" });

    return Results.Json(Item);
});

app.MapDelete("/api/Items/{id:int}", async (int id, ApplicationContext db) =>
{
    Item? Item = await db.Items.FirstOrDefaultAsync(u => u.Id == id);

    if (Item == null) return Results.NotFound(new { message = "Пользователь не найден" });

    db.Items.Remove(Item);
    await db.SaveChangesAsync();
    return Results.Json(Item);
});

app.MapPost("/api/Items", async (Item Item, ApplicationContext db) =>
{
    await db.Items.AddAsync(Item);
    await db.SaveChangesAsync();
    return Item;
});

app.MapPut("/api/Items", async (Item ItemData, ApplicationContext db) =>
{
    var Item = await db.Items.FirstOrDefaultAsync(u => u.Id == ItemData.Id);

    if (Item == null) return Results.NotFound(new { message = "Пользователь не найден" });

    Item.Description = ItemData.Description;
    Item.Name = ItemData.Name;
    Item.Price = ItemData.Price;
    await db.SaveChangesAsync();
    return Results.Json(Item);
});

app.Run();