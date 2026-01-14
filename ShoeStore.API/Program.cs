using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ShoeStore.API.Data;
using ShoeStore.API.Middlewares;
using ShoeStore.API.Models.Validations;
using ShoeStore.API.Repositories.IShoeRepository;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.API.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// ================= DB =================
builder.Services.AddDbContext<ShoeStoreDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================= Controllers =================
builder.Services.AddControllers();

// ================= FluentValidation =================
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ShoeCreateDtoValidator>();

// ================= Swagger =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================= DI =================
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IShoeRepository, ShoeRepository>();
builder.Services.AddScoped<IShoeServices, ShoeServices>();

var app = builder.Build();

// ================= Exception Middleware ================
app.UseMiddleware<ExceptionMiddleware>();

// ================= Swagger =================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ================= Pipeline =================
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
