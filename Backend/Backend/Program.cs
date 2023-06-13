using Backend.Data;
using BlogManager.Data;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
    options.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Example = new OpenApiString("2000-01-01")
    })
);


builder.Services.AddDbContext<AppDbContext>();


var app = builder.Build();

var ctx = app.Services.CreateScope().ServiceProvider.GetService<AppDbContext>();
//DbInitializer.Initialize(ctx);
ctx.Database.EnsureCreated();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(builder => builder.AllowAnyOrigin());

app.Run();

