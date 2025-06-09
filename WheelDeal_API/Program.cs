using Microsoft.EntityFrameworkCore;
using WheelDeal_API;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Repositories;
using WheelDeal_API.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddScoped<ISignUp, SignUpRepository>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
