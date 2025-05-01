using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Project.DAL.Entities;
using Project.Repository;
using Project.Repository.Common;
using Project.Service;
using Project.Service.Common;
using Project.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>();
    });

builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddAutoMapper(typeof(VehicleMakeService).Assembly);

builder.Services.AddScoped<IVehicleMakeRepository, VehicleMakeRepository>();
builder.Services.AddScoped<IVehicleModelRepository, VehicleModelRepository>();
builder.Services.AddScoped<IVehicleOwnerRepository, VehicleOwnerRepository>();
builder.Services.AddScoped<IVehicleEngineTypeRepository, VehicleEngineTypeRepository>();
builder.Services.AddScoped<IVehicleRegistrationRepository, VehicleRegistrationRepository>();

builder.Services.AddScoped<IVehicleMakeService, VehicleMakeService>();
builder.Services.AddScoped<IVehicleModelService, VehicleModelService>();
builder.Services.AddScoped<IVehicleOwnerService, VehicleOwnerService>();
builder.Services.AddScoped<IVehicleEngineTypeService, VehicleEngineTypeService>();
builder.Services.AddScoped<IVehicleRegistrationService, VehicleRegistrationService>();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
