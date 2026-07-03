using FavoritePlacesApi.Data;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend"); //ทำให้ใช้กับ frontend ได้

app.UseAuthorization();

app.MapControllers();

app.Run();
