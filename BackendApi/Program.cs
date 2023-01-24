using BackEndApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt =>
  opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//MySQL as our database service
builder.Services.AddDbContext<ApplicationContext>(
                  options => options
                    .UseMySql(
                      builder.Configuration["ConnectionStrings:DefaultConnection"], 
                      ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
                    )
                  )
                );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
else
{
  app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
