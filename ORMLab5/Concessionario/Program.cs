using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore; // Fundamental
using Microsoft.Extensions.Logging;

namespace Concessionario;


public class Marca
{
    [Key]
    public int Id {get;set;}
    public required string Nom {get;set;}
    public List<Coche> coches {get;set;}

}
public class Coche
{
    [Key]
    public required string Matricula {get; set;}
    public required string Modelo {get;set;}
    public int MarcaId {get;set;}
    public Marca marca {get;set;}
}
    public class Concesionario : DbContext{
        public DbSet<Coche> coches {get;set;}
        public DbSet<Marca> marcas {get;set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Concessionario.sqlite").LogTo(Console.WriteLine,LogLevel.Information);
    }
}

class Program
{
    static void Main(string[] args)
    {
        using (var ctx = new Concesionario())
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();

            var marca1 = new Marca { Nom = "Alpha Romeo" };
            var coche1 = new Coche { Matricula = "1234 ABC", Modelo = "147", marca = marca1, MarcaId = marca1.Id };
            var coche2 = new Coche { Matricula = "5678 DEF", Modelo = "Giulia", marca = marca1, MarcaId = marca1.Id };
            
            using var tx = ctx.Database.BeginTransaction();

            ctx.marcas.Add(marca1);
            ctx.coches.Add(coche1);
            ctx.coches.Add(coche2);
            
            ctx.SaveChanges();
            tx.Commit();

        }
    }
}
