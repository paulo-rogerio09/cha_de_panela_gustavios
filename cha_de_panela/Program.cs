using Microsoft.EntityFrameworkCore;
using cha_de_panela.Data;
using cha_de_panela.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("PermitirFrontend");

// ---- ROTAS DE PRESENTES ----

// 1. Listar todos os presentes
app.MapGet("/presentes", async (AppDbContext db) =>
{
    return await db.Presentes.ToListAsync();
});

// 2. Adicionar um novo presente na lista
app.MapPost("/presentes", async (Presente presente, AppDbContext db) =>
{
    db.Presentes.Add(presente);
    await db.SaveChangesAsync();
    return Results.Created($"/presentes/{presente.Id}", presente);
});

// 3. Reservar/Comprar um presente
app.MapPut("/presentes/{id}/comprar", async (int id, string nomeConvidado, AppDbContext db) =>
{
    var presente = await db.Presentes.FindAsync(id);
    if (presente is null) return Results.NotFound("Presente não encontrado.");
    
    if (presente.Comprado) return Results.BadRequest("Este presente já foi reservado.");

    presente.Comprado = true;
    presente.NomeConvidado = nomeConvidado;
    
    await db.SaveChangesAsync();
    return Results.Ok(presente);
});

// ---- ROTAS DE CONVIDADOS (RSVP) ----

// 4. Confirmar presença
app.MapPost("/convidados", async (Convidado convidado, AppDbContext db) =>
{
    db.Convidados.Add(convidado);
    await db.SaveChangesAsync();
    return Results.Ok("Presença confirmada com sucesso!");
});

// 5. Listar todos os convidados confirmados
app.MapGet("/convidados", async (AppDbContext db) =>
{
    return await db.Convidados.ToListAsync();
});

app.Run();