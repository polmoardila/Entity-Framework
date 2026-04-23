using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gossera;

public class Gos
{
    [Key]
    public required string NumChip {get;set;}
    public DateOnly DataEntradaGossera {get;set;}
    public required string Nom {get;set;}
    public required Gabia gabia {get;set;}
    public int GabiaId {get;set;}
}

public class Gabia
{
    public int Id {get;set;} 
    public int M2 {get;set;}
    public Gos? Gos {get;set;}

}
public class Gossera : DbContext
{
    public DbSet<Gos> Gosseres {get;set;}
    public DbSet<Gabia> Gabies {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite("Data Source=C:\\Users\\Pau\\Documents\\Programacio\\ORMLab\\ORMlab3\\Gossera.sqlite")
            .LogTo(Console.WriteLine, LogLevel.Information);
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new Gossera())
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();

                var gabia1 = new Gabia { M2 = 10 };
                var gabia2 = new Gabia { M2 = 20 };

                var gos1 = new Gos { NumChip = "123", DataEntradaGossera = DateOnly.FromDateTime(DateTime.Now), Nom = "Willy", gabia = gabia1 };
                var gos2 = new Gos { NumChip = "456", DataEntradaGossera = DateOnly.FromDateTime(DateTime.Now), Nom = "Rex", gabia = gabia2 };
                
                using var tx = ctx.Database.BeginTransaction();

                ctx.Gabies.Add(gabia1);
                ctx.Gabies.Add(gabia2);
                ctx.Gosseres.Add(gos1);
                ctx.Gosseres.Add(gos2);
                ctx.SaveChanges();

                tx.Commit();
            }
        }
    }
}
