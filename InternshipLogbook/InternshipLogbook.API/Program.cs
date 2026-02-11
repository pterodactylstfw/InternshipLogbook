using InternshipLogbook.API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Conexiunea la Baza de Date
builder.Services.AddDbContext<InternshipLogbookDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Serviciile API
builder.Services.AddControllers();

// --- SCHIMBAREA ESTE AICI ---
// În loc de AddOpenApi(), folosim sistemul clasic Swashbuckle care are interfață grafică
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ----------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // --- SCHIMBAREA ESTE AICI ---
    // Activăm Swagger (JSON-ul) și SwaggerUI (Pagina Web)
    app.UseSwagger();
    app.UseSwaggerUI(); 
    // ----------------------------
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();