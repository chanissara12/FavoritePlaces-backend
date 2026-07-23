using Domain.Database.Context;
using Domain.Interfaces.ImageManagement;
using Domain.Interfaces.PlacesManagement;
using Domain.Interfaces.UsersManagement;
using Microsoft.EntityFrameworkCore;
using Services.Implements.ImageManagement;
using Services.Implements.PlacesManagement;
using Services.Implements.UsersManagement;
using Web.Handlers;

var builder = WebApplication.CreateBuilder(args);

//ทำให้ใช้กับ frontend ได้
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") 
                  .AllowAnyHeader()  // อนุญาตให้ส่ง Header อะไรมาก็ได้
                  .AllowAnyMethod(); // อนุญาตทุก HTTP Method (GET, POST, PUT, DELETE)
        });
});

// Add services to the container.
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<FavoritePlacesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add ImageService
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPlacesService, PlacesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend"); //ทำให้ใช้กับ frontend ได้

app.UseCustomExceptionHandler(
    app.Services.GetRequiredService<IServiceProvider>());

app.UseAuthorization();

app.MapControllers();

app.Run();
