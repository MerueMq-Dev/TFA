using Microsoft.EntityFrameworkCore;
using TFA.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<ForumDbContext>(options => options
.UseNpgsql(
"User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=forums;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;")
);


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
