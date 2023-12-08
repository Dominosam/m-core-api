using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Mover.Core.Inventory.Interfaces.Services;
using Mover.Core.Inventory.Models.Profiles;
using Mover.Core.Inventory.Services;
using Mover.Core.Watch.Interfaces.Services;
using Mover.Core.Watch.Services;
using Mover.Data.Contexts;
using Mover.Data.HostedServices.Inventory;
using Mover.Data.Interfaces;
using Mover.Data.Repositories.Inventory;
using Mover.Data.Repositories.Watch;
using Redis.OM;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mover API", Version = "v1" });
});

// Register services with dependency injection
builder.Services.AddScoped<IWatchHandsAngleService, WatchHandsAngleService>();
builder.Services.AddScoped<IInventoryItemService, InventoryItemService>();
builder.Services.AddAutoMapper(typeof(InventoryMappingProfile).Assembly);

// Register Redis-related services
builder.Services.AddScoped<IRedisContext>(provider =>
    new RedisContext(configuration));
builder.Services.AddScoped<IWatchHandsRepository, WatchHandsRepository>();

//builder.Services.AddSingleton(new RedisConnectionProvider("redis-19127.c311.eu-central-1-1.ec2.cloud.redislabs.com:19127,password=MoverDataConnectionString"));
//builder.Services.AddHostedService<IndexCreationService>();

// Register MongoDB-related services
builder.Services.AddScoped<IMongoDbContext>(provider =>
    new MongoDbContext(configuration));
builder.Services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();


// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mover API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();