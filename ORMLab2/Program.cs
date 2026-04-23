namespace ORMLab2;

using System.ComponentModel.DataAnnotations;
using System.Data.Common;

public class Producte
{
    [Key]
    public string EAN13 { get; set; } = Guid.NewGuid().ToString()[0..13];
    public string Nom { get; set; } = "";
    public decimal Preu { get; set; }

}

public class Factura
{

    public int Numero { get; set; }
    [Key]
    public int Exercici { get; set; }
    public string Descripcio { get; set; } = ""; 
}
public class LiniaProducte
{
    public int ID { get; set; }
    public Producte producte { get; set; } = default!;
    public Factura factura { get; set; } = default!;
    public int Quantitat { get; set; }
    public int PreuDeVenda { get; set; }
    public int Descompte { get; set; }
    public decimal Total { get; set; }
}

public class FacturacioCtx : DbContext
{
    public DbSet<Producte> Productes { get; set; } = default!;
    public DbSet<Factura> Factures { get; set; } = default!;
    public DbSet<LiniaProducte> Linies { get; set; } = default!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
        .UseSqlite("Data Source=C:\\Users\\Pau\\Documents\\Programacio\\ORMLab\\Facturacio.db")
        .LogTo(System.Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        
    }
}
class Program
{
    static void Main(string[] args)
    {
        using var ctx = new FacturacioCtx();     
            
            ctx.Database.EnsureCreated();
    }
}
