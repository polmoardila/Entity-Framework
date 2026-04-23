// using System.ComponentModel.DataAnnotations;
// using Microsoft.EntityFrameworkCore;

// namespace ORMLab;

// class Program
// {
//     static void Main(string[] args)
//     {

//         // El que hauriem de fer si no fesim servir LINQ:

//         // static bool ElMeuFiltre(int item)
//         // {
//         //     if (item > 5)
//         //     {
//         //         return true;
//         //     }
//         //     else
//         //     {
//         //         return false;
//         //     }
//         // }

//                 // Amb LINQ, podem fer-ho d'una manera molt més elegant i senzilla:

//                 // List<int> LaMevaLlista = [55,43,17,2,3,4,5,6,7,8,9];

//                 // var xxx = LaMevaLlista.Where(item => item > 5);

                
//                 List<Alumne> LaMevaLlista = [

//                     new Alumne() { Nom = "Joan", Curs = "DAM", Edat = 20, DarreraActualitzacio = DateTime.Now },
//                     new Alumne() { Nom = "Maria", Curs = "DAW", Edat = 22, DarreraActualitzacio = DateTime.Now },
//                     new Alumne() { Nom = "Pere", Curs = "DAM", Edat = 19, DarreraActualitzacio = DateTime.Now },
//                     new Alumne() { Nom = "Anna", Curs = "DAW", Edat = 21, DarreraActualitzacio = DateTime.Now },
                
//                 ];

//         using var ctx = new InstitutDbCtx();     
            
//             ctx.Database.EnsureCreated();

//             ctx.Alumnes.AddRange(LaMevaLlista);

//             ctx.SaveChanges();
           
        
//                 var xxx = 
//                 LaMevaLlista
//                 .Where(item => item.Edat >= 18)
//                 .ToList()
//                 .OrderBy( item => item.Curs)
//                 .ThenByDescending(x => x.Edat);

    
//     }
//     public class Alumne{
        
//         public string Curs { get; set; } = "";
//         public int Edat { get; set; }
//         public DateTime DarreraActualitzacio { get; set; }

//         [Key]
//         public string Nom { get; set; } = "";
      

       

//     }

//     public class InstitutDbCtx : DbContext
//     {
//         public DbSet<Alumne> Alumnes { get; set; }

//             protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//             {
//                 optionsBuilder
//                 .UseSqlite("Data Source=C:\\Users\\Pau\\Documents\\Programacio\\ORMLab\\MyDatabase.db")
//                 .LogTo(System.Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
                
//             }



//     }
// }
