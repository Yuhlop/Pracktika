using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Prackticheskaya_2024;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite("DataSource = AppData/database.sqlite"));

// Add services to the container.
builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.01",
        Title = "Web-server",
        Description = "Ох, зря я туда полез...",
        TermsOfService = new Uri("http://scufozavriki.ru"),
        Contact = new OpenApiContact
        {
            Name = "Без контактов",
            Url = new Uri("http://scufozavriki.ru")
        },
        License = new OpenApiLicense
        {
            Name = "Без лицензии",
            Url = new Uri("http://scufozavriki.ru")
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
