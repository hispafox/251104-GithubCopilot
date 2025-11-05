using Microsoft.EntityFrameworkCore;
using DemoWinForms.Domain.Entities;
using DemoWinForms.Domain.Enums;

namespace DemoWinForms.Data;

/// <summary>
/// Contexto de base de datos para la aplicación
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
     : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
  public DbSet<Tarea> Tareas => Set<Tarea>();
    public DbSet<Etiqueta> Etiquetas => Set<Etiqueta>();
    public DbSet<TareaEtiqueta> TareasEtiquetas => Set<TareaEtiqueta>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Usuario
      modelBuilder.Entity<Usuario>(entity =>
    {
        entity.ToTable("Usuarios");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Nombre)
       .IsRequired()
            .HasMaxLength(200);
   
            entity.Property(e => e.Email)
        .IsRequired()
          .HasMaxLength(255);
      
entity.HasIndex(e => e.Email)
        .IsUnique();
    
            entity.Property(e => e.Telefono)
     .IsRequired()
   .HasMaxLength(20);
       
         entity.Property(e => e.Pais)
            .IsRequired()
     .HasMaxLength(100);
  });

        // Configuración de Tarea
      modelBuilder.Entity<Tarea>(entity =>
        {
       entity.ToTable("Tareas");
entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Titulo)
       .IsRequired()
                .HasMaxLength(200);
     
    entity.Property(e => e.Descripcion)
     .HasMaxLength(2000);
     
   entity.Property(e => e.FechaCreacion)
  .IsRequired();
           
    entity.Property(e => e.Prioridad)
        .IsRequired()
          .HasConversion<int>();
       
            entity.Property(e => e.Estado)
        .IsRequired()
                .HasConversion<int>();
      
            entity.Property(e => e.Categoria)
     .IsRequired()
                .HasMaxLength(50);
         
    entity.Property(e => e.UltimaModificacion)
           .IsRequired();
             
        entity.Property(e => e.EliminadoLogico)
 .HasDefaultValue(false);
     
   // Índices para optimización
            entity.HasIndex(e => e.Estado);
            entity.HasIndex(e => e.Prioridad);
    entity.HasIndex(e => e.FechaVencimiento);
            entity.HasIndex(e => e.UsuarioId);
        entity.HasIndex(e => e.EliminadoLogico);
   
         // Relación con Usuario
       entity.HasOne(e => e.Usuario)
 .WithMany()
        .HasForeignKey(e => e.UsuarioId)
      .OnDelete(DeleteBehavior.Restrict);
            
            // Relación auto-referencial (Subtareas)
entity.HasOne(e => e.TareaPadre)
                .WithMany(e => e.Subtareas)
 .HasForeignKey(e => e.TareaPadreId)
         .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Etiqueta
        modelBuilder.Entity<Etiqueta>(entity =>
        {
            entity.ToTable("Etiquetas");
  entity.HasKey(e => e.Id);
    
          entity.Property(e => e.Nombre)
        .IsRequired()
        .HasMaxLength(50);
                
   entity.Property(e => e.ColorHex)
      .HasMaxLength(7)
                .HasDefaultValue("#808080");
            
       entity.HasIndex(e => e.Nombre)
       .IsUnique();
        });

        // Configuración de TareaEtiqueta (many-to-many)
        modelBuilder.Entity<TareaEtiqueta>(entity =>
        {
   entity.ToTable("TareasEtiquetas");
      entity.HasKey(te => new { te.TareaId, te.EtiquetaId });
   
            entity.HasOne(te => te.Tarea)
           .WithMany(t => t.TareaEtiquetas)
.HasForeignKey(te => te.TareaId)
    .OnDelete(DeleteBehavior.Cascade);
    
     entity.HasOne(te => te.Etiqueta)
    .WithMany(e => e.TareaEtiquetas)
                .HasForeignKey(te => te.EtiquetaId)
         .OnDelete(DeleteBehavior.Cascade);
});

        // Datos semilla
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
     // Usuario por defecto
    modelBuilder.Entity<Usuario>().HasData(
  new Usuario
      {
    Id = 1,
      Nombre = "Usuario Demo",
     Edad = 30,
       Pais = "España",
     Email = "demo@tareas.com",
    Telefono = "123456789"
 }
        );

        // Etiquetas predeterminadas
     modelBuilder.Entity<Etiqueta>().HasData(
    new Etiqueta { Id = 1, Nombre = "Urgente", ColorHex = "#DC3545" },
      new Etiqueta { Id = 2, Nombre = "Proyecto", ColorHex = "#007BFF" },
            new Etiqueta { Id = 3, Nombre = "Personal", ColorHex = "#28A745" },
            new Etiqueta { Id = 4, Nombre = "Trabajo", ColorHex = "#FFC107" },
            new Etiqueta { Id = 5, Nombre = "Importante", ColorHex = "#FF5733" }
        );

        // Tareas de ejemplo
modelBuilder.Entity<Tarea>().HasData(
     new Tarea
        {
                Id = 1,
       Titulo = "Bienvenido al Gestor de Tareas",
      Descripcion = "Esta es tu primera tarea de ejemplo. Puedes editarla o eliminarla.",
         FechaCreacion = DateTime.Now,
        FechaVencimiento = DateTime.Now.AddDays(7),
       Prioridad = PrioridadTarea.Media,
            Estado = EstadoTarea.Pendiente,
     Categoria = "Personal",
  UsuarioId = 1,
      UltimaModificacion = DateTime.Now,
  EliminadoLogico = false
          }
        );
 }
}
