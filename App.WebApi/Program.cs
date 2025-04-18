using App.Data;
using App.Data.Contexts;
using App.WebApi.Services;
using App.Shared.Util.ExceptionHandling;
using App.Shared;
using App.Data.Seeding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbRegistrations(builder.Configuration);
builder.Services.AddApiServiceCollections(builder.Configuration);
builder.Services.AddSharedServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await DataSeeder.SeedDatabaseAsync(app.Services);

app.UseCors("AllowAll");

app.UseCustomExceptionHandler();

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
