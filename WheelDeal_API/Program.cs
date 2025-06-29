using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WheelDeal_API;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Repositories;
using WheelDeal_API.Repositories.Interface;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<ISignUp, SignUpRepository>();
builder.Services.AddScoped<ISignIn, SignInRepository>();
builder.Services.AddScoped<IBodyType, BodyTypeRepository>();
builder.Services.AddScoped<IBrand, BrandRepository>();
builder.Services.AddScoped<IDriveType, DriveTypeRepository>();
builder.Services.AddScoped<IFuelType, FuelTypeRepository>();
builder.Services.AddScoped<IModels, ModelRepository>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDb")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("EnableCORS");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
