using GymMembership.Models;
using GymMembership.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>(
    options => options.UseNpgsql(builder.Configuration["Database:ConnectionString"])
);

builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, JWTService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
