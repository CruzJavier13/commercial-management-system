using CommercialSystem.Shared.Persistence.Database;
using Mod.Products.Application.UseCases;
using Mod.Products.Domain.Repositories;
using Mod.Products.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS policy specifically for your Angular frontend (typically port 4200)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular default local URL - adjust if your Angular app runs on a different port or domain
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Retrieve the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

// Register the ISqlDbContext as Scoped (one instance per HTTP Request from Angular)
builder.Services.AddScoped<ISqlDbContext>(provider => new SqlDbContext(connectionString));

// Register Module Dependencies (Dependency Injection Mapping)
// Products Module
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<CreateProductUseCase>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
