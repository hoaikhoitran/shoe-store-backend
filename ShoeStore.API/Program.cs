using Microsoft.EntityFrameworkCore;
using ShoeStore.API.Data;
using ShoeStore.API.Repositories.IShoeRepository;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.API.Services.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ShoeStoreDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IShoeRepository, ShoeRepository>();
builder.Services.AddScoped<IShoeServices, ShoeServices>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
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
app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapControllers();

app.Run();

