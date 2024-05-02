using connect_cic_api.Domain;
using Microsoft.EntityFrameworkCore;

namespace connect_cic_api.Infra.Persistence;

public class ConnectCICAPIContext : DbContext
{

    public ConnectCICAPIContext(DbContextOptions<ConnectCICAPIContext> options) : base(options)
      {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
      }

    public DbSet<User> Users { get; set; }
    public DbSet<VacancyType> VacancyTypes { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Professor> Professors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        #region SqLite ConnectionString
        var StringConnection = "Data Source=vacancies.db";
        optionsBuilder.UseSqlite(StringConnection)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        #endregion
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

            # region Enities Configuration
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(c => c.UserID);
            modelBuilder.Entity<User>()
              .HasOne(u => u.Professor)
              .WithOne(a => a.User)
              .HasForeignKey<Professor>(a => a.UserID)
              .IsRequired(false);
            modelBuilder.Entity<User>()
              .HasOne(u => u.Student)
              .WithOne(a => a.User)
              .HasForeignKey<Student>(a => a.UserID)
              .IsRequired(false);

            modelBuilder.Entity<VacancyType>().ToTable("VacancyTypes");
            modelBuilder.Entity<VacancyType>().HasKey(c => c.VacancyTypeID);

            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Student>().HasKey(a => a.StudentID);



            modelBuilder.Entity<Professor>().ToTable("Professors");
            modelBuilder.Entity<Professor>().HasKey(p => p.ProfessorID);
            

            modelBuilder.Entity<Vacancy>().ToTable("Vacancies");
            modelBuilder.Entity<Vacancy>().HasKey(v => v.VacancyID);
            modelBuilder.Entity<Vacancy>().HasOne(v => v.Professor).WithMany(p => p.Vacancies).HasForeignKey(v => v.ProfessorID);
            modelBuilder.Entity<Vacancy>().HasOne(v => v.VacancyType).WithMany(t => t.Vacancies).HasForeignKey(v => v.VacancyTypeID);
            modelBuilder.Entity<Vacancy>().HasMany(v => v.Students).WithMany(a => a.Vacancies);
            #endregion


            # region seed data
            modelBuilder.Entity<VacancyType>().HasData(
                new VacancyType { VacancyTypeID = 1, Name = "Estágio" },
                new VacancyType { VacancyTypeID = 2, Name = "Iniciação Cientifica" },
                new VacancyType { VacancyTypeID = 3, Name = "Iniciação a docencia" }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student { StudentID = 1, 
                            Name = "Everaldina Barbosa", 
                            Email = "everaldina@gmail.com",
                            Course = "Ciencia da Computação",
                            CRAA = 9f,
                            Status = "Cursando",
                },
                new Student { StudentID = 2, 
                            Name = "Lavinia", 
                            Email = "lavinia@gmail.com",
                            Course = "Ciencia da Computação",
                            CRAA = 9f,
                            Status = "Cursando",
                },
                new Student { StudentID = 3, 
                            Name = "ana cristina", 
                            Email = "ana_cristina@gmail.com",
                            Course = "Ciencia da Computação",
                            CRAA = 9f,
                            Status = "Cursando",
                },
                new Student { StudentID = 4, 
                            Name = "gabie", 
                            Email = "gabie@gmail.com",
                            Course = "Ciencia da Computação",
                            CRAA = 9f,
                            Status = "Cursando",
                }
            );

            modelBuilder.Entity<Professor>().HasData(
                new Professor { ProfessorID = 1, 
                                Name = "Helder", 
                                Email = "helder@uesc.com",
                                Department = "DCET",
                },
                new Professor { ProfessorID = 2, 
                                Name = "Martha", 
                                Email = "martinha@uesc.com",
                                Department = "DCET",
                },
                new Professor { ProfessorID = 3, 
                                Name = "Bravo", 
                                Email = "bravo@uesc.com",
                                Department = "DCET",
                }
                );

            modelBuilder.Entity<Vacancy>().HasData(
                new Vacancy { VacancyID = 1, 
                            Value = 1000f, 
                            StartDate = DateTime.Now, 
                            EndDate = DateTime.Now.AddDays(30), 
                            Requirements = "Conhecimento em Java", 
                            Description = "Desenvolvimento de aplicação web", 
                            ProjectTitle = "Sistema de gerenciamento de vendas", 
                            Status = "Aberta", 
                            ProfessorID = 1, 
                            VacancyTypeID = 1
                },
                new Vacancy { VacancyID = 2, 
                            Value = 1000f, 
                            StartDate = DateTime.Now, 
                            EndDate = DateTime.Now.AddDays(30), 
                            Requirements = "Conhecimento em Java", 
                            Description = "Desenvolvimento de aplicação web", 
                            ProjectTitle = "Sistema de gerenciamento de vendas", 
                            Status = "Aberta", 
                            ProfessorID = 2, 
                            VacancyTypeID = 1
                },
                new Vacancy { VacancyID = 3, 
                            Value = 1000f, 
                            StartDate = DateTime.Now, 
                            EndDate = DateTime.Now.AddDays(30), 
                            Requirements = "Conhecimento em Java", 
                            Description = "Desenvolvimento de aplicação web", 
                            ProjectTitle = "Sistema de gerenciamento de vendas", 
                            Status = "Aberta", 
                            ProfessorID = 3, 
                            VacancyTypeID = 2
                }
            );
            # endregion
      }
}
