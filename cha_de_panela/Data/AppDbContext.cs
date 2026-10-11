using Microsoft.EntityFrameworkCore;
using cha_de_panela.Models;

namespace cha_de_panela.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Presente> Presentes { get; set; }
    public DbSet<Convidado> Convidados { get; set; }
}