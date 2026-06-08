using CommercialSystem.Shared.Persistence.Database;
using Mod.Products.Infrastructure;
using Mod.Emp.Infrastructure;
using Mod.Customers.Infrastructure;
using Mod.Billing.Infrastructure;
using Mod.Inventory.Infrastructure;
using Mod.Sales.Infrastructure;


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

builder.Services.AddSingleton<ISqlDbContext, SqlDbContext>();


// Register Module Dependencies (Dependency Injection Mapping)
// Employees Module
builder.Services.AddEmployeesModule();

// Products Module
builder.Services.AddProductsModule();

// Bills Module
builder.Services.AddBillingModule();

// Inventory Module
builder.Services.AddInventoryModule();

// Customers Module
builder.Services.AddCustomersModule();

// Sales Module
builder.Services.AddSalesModule();

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
