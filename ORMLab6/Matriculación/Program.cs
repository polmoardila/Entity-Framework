using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Matriculación;

public class Estudiante
{
    [Key]
    public int id {get;set;}
    public required string Nombre {get;set;}   
    public List<Curso> cursos {get;set;} = new();
}

public class Curso
{
    [Key]
    public int id {get;set;}
    public required string NombreCurso {get;set;}
    public List<Estudiante> estudiantes {get;set;}    
}


public class Matriculación : DbContext{

    public DbSet<Estudiante> estudiantes {get;set;}
    public DbSet<Curso> cursos {get;set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Matriculacion.sqlite").LogTo(Console.WriteLine,LogLevel.Information);
    }
}
    


class Program
{
    static void Main(string[] args)
    {
        using (var ctx = new Matriculación())
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();


            var curso1 = new Curso{id= 1, NombreCurso= "Programación"};
            var curso2 = new Curso{id= 2, NombreCurso= "Bases de Datos"};

            var estudiante1 = new Estudiante{id= 1, Nombre= "Ana", cursos= new List<Curso>{curso1, curso2}};
            var estudiante2 = new Estudiante{id= 2, Nombre= "Bruno", cursos= new List<Curso>{curso1}};

            using var tx = ctx.Database.BeginTransaction();

            ctx.cursos.Add(curso1);
            ctx.cursos.Add(curso2);
            ctx.estudiantes.Add(estudiante1);
            ctx.estudiantes.Add(estudiante2);
            
            ctx.SaveChanges();
            tx.Commit();

        }
    }
}
