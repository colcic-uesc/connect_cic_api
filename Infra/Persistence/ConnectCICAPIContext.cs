namespace connect_cic_api.Infra.Persistence;

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
    public DbSet<Vaga> Vagas { get; set; }
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

      }
}
