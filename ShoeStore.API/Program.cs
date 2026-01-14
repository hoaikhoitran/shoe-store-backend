using Microsoft.EntityFrameworkCore;
using ShoeStore.API.Data;
using ShoeStore.API.Repositories.IShoeRepository;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ShoeStoreDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IShoeRepository, ShoeRepository>();




var app = builder.Build();

// Configure the HTTP request pipeline.
//
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ShoeStoreDbContext>();
    Console.WriteLine(db.Database.CanConnect()
        ? "Connected to DB"
        : "Cannot connect to DB");
}
//
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

