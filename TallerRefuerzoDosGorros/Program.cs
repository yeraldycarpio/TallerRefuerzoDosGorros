using Microsoft.EntityFrameworkCore;
using TallerRefuerzoDosGorros.Endpoints;
using TallerRefuerzoDosGorros.Models.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GDbContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

builder.Services.AddScoped<GorrosDAL>();
var app = builder.Build();

app.AddGorroEndpoints();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();