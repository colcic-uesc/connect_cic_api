namespace connect_cic_api.Infra.Persisence;

using Microsoft.EntityFrameworkCore;

public class ConnectCICAPIContext : DbContext
{

    public ConnectCICAPIContext(DbContextOptions<ConnectCICAPIContext> options) : base(options)
      {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
      }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        #region SqLite ConnectionString
        var StringConnection = "Data Source=uesc_courses.db";
        optionsBuilder.UseSqlite(StringConnection)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        #endregion
    }
}
