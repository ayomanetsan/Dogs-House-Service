using AspNetCoreRateLimit;
using DogsHouseService.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDogsHouseServiceDbContext(builder.Configuration);
builder.Services.RegisterCustomServices();
builder.Services.AddControllers();
builder.Services.AddIpRateLimiting(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseIpRateLimiting();

app.MapControllers();

app.Run();
