﻿namespace connect_cic_api.Infra.Persistence;

using connect_cic_api.Domain;
using Microsoft.EntityFrameworkCore;

public class ConnectCICAPIContext : DbContext
{

    public ConnectCICAPIContext(DbContextOptions<ConnectCICAPIContext> options) : base(options)
      {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
      }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<TipoVaga> TipoVagas { get; set; }
    public DbSet<Vaga> Vagas { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        #region SqLite ConnectionString
        var StringConnection = "Data Source=vagas.db";
        optionsBuilder.UseSqlite(StringConnection)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        #endregion
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Usuario>().HasKey(c => c.UsuarioID);

            modelBuilder.Entity<TipoVaga>().ToTable("TiposVaga");
            modelBuilder.Entity<TipoVaga>().HasKey(c => c.VagaTipoID);

            modelBuilder.Entity<Aluno>().ToTable("Alunos");
            modelBuilder.Entity<Aluno>().HasKey(a => a.AlunoID);
            modelBuilder.Entity<Aluno>().HasOne(a => a.Usuario).WithOne(u => u.Aluno).HasForeignKey<Aluno>(a => a.UsuarioID);

            modelBuilder.Entity<Professor>().ToTable("Professores");
            modelBuilder.Entity<Professor>().HasKey(p => p.ProfessorID);
            modelBuilder.Entity<Professor>().HasOne(p => p.Usuario).WithOne(u => u.Professor).HasForeignKey<Professor>(p => p.UsuarioID);

            modelBuilder.Entity<Vaga>().ToTable("Vagas");
            modelBuilder.Entity<Vaga>().HasKey(v => v.VagaID);
            modelBuilder.Entity<Vaga>().HasOne(v => v.Professor).WithMany(p => p.Vagas).HasForeignKey(v => v.ProfessorID);
            modelBuilder.Entity<Vaga>().HasOne(v => v.TipoVaga).WithMany(t => t.Vagas).HasForeignKey(v => v.TipoVagaID);
            modelBuilder.Entity<Vaga>().HasMany(v => v.Alunos).WithMany(a => a.Vagas);

      }
}
