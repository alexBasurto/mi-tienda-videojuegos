using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApiTiendaVideojuegos.Models;

public partial class MiTiendaVideojuegosContext : DbContext
{
    public MiTiendaVideojuegosContext(DbContextOptions<MiTiendaVideojuegosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorias> Categorias { get; set; }

    public virtual DbSet<Desarrolladoras> Desarrolladoras { get; set; }

    public virtual DbSet<Juegos> Juegos { get; set; }

    public virtual DbSet<Plataformas> Plataformas { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorias>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Genero)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("genero");
            entity.Property(e => e.Subgenero)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("subgenero");
        });

        modelBuilder.Entity<Desarrolladoras>(entity =>
        {
            entity.HasKey(e => e.IdDesarrolladora);

            entity.Property(e => e.IdDesarrolladora).HasColumnName("idDesarrolladora");
            entity.Property(e => e.Indie).HasColumnName("indie");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Pais)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pais");
        });

        modelBuilder.Entity<Juegos>(entity =>
        {
            entity.HasKey(e => e.IdJuego);

            entity.Property(e => e.IdJuego).HasColumnName("idJuego");
            entity.Property(e => e.Caratula).HasColumnName("caratula");
            entity.Property(e => e.Disponible).HasColumnName("disponible");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.IdDesarrolladora).HasColumnName("idDesarrolladora");
            entity.Property(e => e.IdPlataforma).HasColumnName("idPlataforma");
            entity.Property(e => e.Lanzamiento)
                .HasColumnType("datetime")
                .HasColumnName("lanzamiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Pegi).HasColumnName("pegi");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Juegos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Juegos_Categorias");

            entity.HasOne(d => d.IdDesarrolladoraNavigation).WithMany(p => p.Juegos)
                .HasForeignKey(d => d.IdDesarrolladora)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Juegos_Desarrolladoras");

            entity.HasOne(d => d.IdPlataformaNavigation).WithMany(p => p.Juegos)
                .HasForeignKey(d => d.IdPlataforma)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Juegos_Plataformas");
        });

        modelBuilder.Entity<Plataformas>(entity =>
        {
            entity.HasKey(e => e.IdPlataforma);

            entity.Property(e => e.IdPlataforma).HasColumnName("idPlataforma");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__3214EC0702675DDB");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
