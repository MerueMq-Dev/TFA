using Microsoft.EntityFrameworkCore;
using TFA.Domain;
using TFA.Domain.Authorization;
using TFA.Domain.Identity;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Storage;
using TFA.Storage.Storages;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Postgres");

// Add services to the container.

builder.Services.AddScoped<IGetForumsUseCase, GetForumsUseCase>();
builder.Services.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();
builder.Services.AddScoped<IMomentProvider, MomentProvider>();
builder.Services.AddScoped<IGuidFactory, GuidFactory>();
builder.Services.AddScoped<ICreateTopicStorage, CreateTopicStorage>();
builder.Services.AddScoped<IGetForumsStorage, GetForumsStorage>();
builder.Services.AddScoped<IIntentionResolver, TopicIntentionResolver>();
builder.Services.AddScoped<IIntentionManager, IntentionManager>();
builder.Services.AddScoped<IIdentityProvider, IdentityProvider>();
builder.Services.AddScoped<IGuidFactory, GuidFactory>();
builder.Services.AddScoped<IMomentProvider, MomentProvider>();

builder.Services.AddDbContext<ForumDbContext>(options => options
    .UseNpgsql(connectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
