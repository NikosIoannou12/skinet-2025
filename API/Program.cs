using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//Dependency Injection of the Generic Repository (Type of IGenericRepository<>)
//So every time we "ask" for IGenericRepository<T> it will return an instance of GenericRepository<T>
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 
//AddScoped - runs for the duration of the HTTP Request
builder.Services.AddScoped<IProductRepository, ProductsRepository>();

var app = builder.Build();

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
} catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();
