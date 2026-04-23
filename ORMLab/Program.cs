using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ORMB2;

public class Producte
{
    [Key]
    public string EAN13 { get; set; } = "";
    public string Nom { get; set; } = "";
    public decimal Preu { get; set; }
}

public class Factura
{
    [Key]
    public int Numero { get; set; }
    public int Exercici { get; set; }
    public string Descripcio { get; set; } = "";
    public decimal Total { get; set; }
    public List<LiniaFactura> LiniesDeFactura { get; set; } = [];
}

public class LiniaFactura
{
    public int Id { get; set; }
    public Producte Producte { get; set; } = default!;
    public Factura Factura { get; set; } = default!;
    public int Quantitat { get; set; }
    public decimal PreuVenta { get; set; }
    public int PctDescompte { get; set; } = 0;
    public decimal TotalLinia { get; set; }
}

public class FacturacioCtx : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
        .UseSqlite("Data Source=C:\\Users\\Pau\\Documents\\Programacio\\ORMLab\\ORMLab\\Facturacio.sqlite")
        .LogTo(System.Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
        .EnableSensitiveDataLogging();
    }
    public DbSet<Factura> Factures { get; set; } = default!;
    public DbSet<LiniaFactura> LiniesDeFactura { get; set; } = default!;
    public DbSet<Producte> Productes { get; set; } = default!;
}

class Program
{
    static void Main(string[] args)
    {
        using var ctx = new FacturacioCtx();
        var n =
            ctx
            .Factures
            .Select(f => new
            {
                numero = f.Numero,
                linies = f.LiniesDeFactura.Max(lf => lf.PreuVenta)
            })
            .ToList();
    }

    private static void EsborraLaFacturaNumero4()
    {
        using var ctx = new FacturacioCtx();

        var f4 = ctx.Factures.Find(4);

        ctx.Factures.Remove(f4!);

        ctx.SaveChanges();
    }

    private static void ActualitzaFactura4()
    {
        // Factures que contenen el número 4 a la descripció
        var ctx = new FacturacioCtx();
        var factures4 = ctx.Factures.Where(f => f.Numero == 4).First();

        factures4.Descripcio = "Aquesta és la factura número 4 si o si";

        ctx.SaveChanges();
    }

    private static void Crea1000Factures()
    {
        using var ctx = new FacturacioCtx();
        ctx.Database.EnsureCreated();

        for (int n = 0; n < 1000; n++)
        {
            var p1 = CreaProducte($"Producte {n}", n * 3.2m);
            var f1 = CreaFactura($"Mi factura {n}");
            CreaLinia(f1, p1, quantitat: n + 1, preuventa: n * 3m);
            CreaLinia(f1, p1, quantitat: n + 4, preuventa: n * 8m);
            CreaLinia(f1, p1, quantitat: 2 * n, preuventa: n * 5m);


            ctx.Factures.Add(f1);
            ctx.Productes.Add(p1);
        }

        ctx.SaveChanges();
    }

    private static void CreaLinia(Factura factura, Producte producte, int quantitat, decimal preuventa, int pctdescompte = 0)
    {
        var l = new LiniaFactura()
        {
            Factura = factura,
            Producte = producte,
            Quantitat = quantitat,
            PreuVenta = preuventa,
            PctDescompte = pctdescompte,
            TotalLinia = (preuventa * quantitat) * (1 - (pctdescompte / 100.0m))
        };

        factura.LiniesDeFactura.Add(l);
        factura.Total += l.TotalLinia;
    }

    private static Producte CreaProducte(string nom, decimal preu)
    {
        return new()
        {
            EAN13 = Guid.NewGuid().ToString().Replace("-", "")[^13..],
            Nom = nom,
            Preu = preu,
        };
    }

    static public Factura CreaFactura(string descripcio)
    {
        return new Factura()
        {
            Descripcio = descripcio,
            Exercici = DateTime.Now.Year,
            Total = 0
        };
    }
}
