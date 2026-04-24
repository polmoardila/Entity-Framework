using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore; // Te faltaba esta
using Microsoft.Extensions.Logging;

namespace gimnasio;

public class Socio
{
    [Key]
    public required string DNI { get; set; }
    public required string Nombre { get; set; }
    public required Taquilla Taquilla { get; set; }

    public required int idTaquilla { get; set; }
}

public class Taquilla
{
    [Key]
    public int Id { get; set; }
    public int Numero { get; set; }
    public Socio? Socio { get; set; }
}

public class GimnasioCtx : DbContext
{
    public DbSet<Socio> Socios { get; set; }
    public DbSet<Taquilla> Taquillas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Nota: Es .UseSqlite (con l minúscula)
        optionsBuilder
            .UseSqlite("Data Source=Gimnasio.sqlite")
            .LogTo(Console.WriteLine, LogLevel.Information);
    }
}

class Program
{
    static void Main(string[] args)
    {
        using (var ctx = new GimnasioCtx()) // Nombre corregido
        {
            ctx.Database.EnsureDeleted(); // Recomendado para pruebas
            ctx.Database.EnsureCreated();

            var taquilla1 = new Taquilla { Numero = 42 };
            var socio1 = new Socio { DNI = "12345678A", Nombre = "Marc", Taquilla = taquilla1 ,idTaquilla = taquilla1.Id};

            // 1. Iniciamos transacción
            using var tx = ctx.Database.BeginTransaction();

            // 2. Añadimos los objetos
            ctx.Socios.Add(socio1); // Al añadir al socio, EF añade la taquilla automáticamente
            
            // 3. Guardamos y confirmamos
            ctx.SaveChanges();
            tx.Commit();
            
            Console.WriteLine("¡Socio y Taquilla guardados!");
        }
    }
}