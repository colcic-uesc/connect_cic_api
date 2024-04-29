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
    public DbSet<TipoVaga> TipoVagas { get; set; }
    public DbSet<Vaga> Vagas { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Professor> Professores { get; set; }
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

            # region Enities Configuration
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Usuario>().HasKey(c => c.UsuarioID);
            modelBuilder.Entity<Usuario>()
              .HasOne(u => u.Professor)
              .WithOne(a => a.Usuario)
              .HasForeignKey<Professor>(a => a.UsuarioID)
              .IsRequired(false);
            modelBuilder.Entity<Usuario>()
              .HasOne(u => u.Aluno)
              .WithOne(a => a.Usuario)
              .HasForeignKey<Aluno>(a => a.UsuarioID)
              .IsRequired(false);

            modelBuilder.Entity<TipoVaga>().ToTable("TiposVaga");
            modelBuilder.Entity<TipoVaga>().HasKey(c => c.VagaTipoID);

            modelBuilder.Entity<Aluno>().ToTable("Alunos");
            modelBuilder.Entity<Aluno>().HasKey(a => a.AlunoID);



            modelBuilder.Entity<Professor>().ToTable("Professores");
            modelBuilder.Entity<Professor>().HasKey(p => p.ProfessorID);
            

            modelBuilder.Entity<Vaga>().ToTable("Vagas");
            modelBuilder.Entity<Vaga>().HasKey(v => v.VagaID);
            modelBuilder.Entity<Vaga>().HasOne(v => v.Professor).WithMany(p => p.Vagas).HasForeignKey(v => v.ProfessorID);
            modelBuilder.Entity<Vaga>().HasOne(v => v.TipoVaga).WithMany(t => t.Vagas).HasForeignKey(v => v.TipoVagaID);
            modelBuilder.Entity<Vaga>().HasMany(v => v.Alunos).WithMany(a => a.Vagas);
            #endregion


            # region seed data
            modelBuilder.Entity<TipoVaga>().HasData(
                new TipoVaga { VagaTipoID = 1, Nome = "Estágio" },
                new TipoVaga { VagaTipoID = 2, Nome = "Iniciação Cientifica" },
                new TipoVaga { VagaTipoID = 3, Nome = "Iniciação a docencia" }
            );

            modelBuilder.Entity<Aluno>().HasData(
                new Aluno { AlunoID = 1, 
                            Nome = "Everaldina Barbosa", 
                            EmailContato = "everaldina@gmail.com",
                            Curso = "Ciencia da Computação",
                            CRAA = 9f,
                            Status = "Cursando",
                },
                new Aluno { AlunoID = 2, 
                            Nome = "Lavinia", 
                            EmailContato = "lavinia@gmail.com",
                            Curso = "Ciencia da Computação",
                            CRAA = 9f,
                            Status = "Cursando",
                },
                new Aluno { AlunoID = 3, 
                            Nome = "ana cristina", 
                            EmailContato = "ana_cristina@gmail.com",
                            Curso = "Ciencia da Computação",
                            CRAA = 9f,
                            Status = "Cursando",
                },
                new Aluno { AlunoID = 4, 
                            Nome = "gabie", 
                            EmailContato = "gabie@gmail.com",
                            Curso = "Ciencia da Computação",
                            CRAA = 9f,
                            Status = "Cursando",
                }
            );

            modelBuilder.Entity<Professor>().HasData(
                new Professor { ProfessorID = 1, 
                                Nome = "Helder", 
                                EmailContato = "helder@uesc.com",
                                Departamento = "DCET",
                },
                new Professor { ProfessorID = 2, 
                                Nome = "Martha", 
                                EmailContato = "martinha@uesc.com",
                                Departamento = "DCET",
                },
                new Professor { ProfessorID = 3, 
                                Nome = "Bravo", 
                                EmailContato = "bravo@uesc.com",
                                Departamento = "DCET",
                }
                );

            modelBuilder.Entity<Vaga>().HasData(
                new Vaga { VagaID = 1, 
                            Valor = 1000f, 
                            DataInicio = DateTime.Now, 
                            DataFim = DateTime.Now.AddDays(30), 
                            Requisitos = "Conhecimento em Java", 
                            Descricao = "Desenvolvimento de aplicação web", 
                            TituloProjeto = "Sistema de gerenciamento de vendas", 
                            Status = "Aberta", 
                            ProfessorID = 1, 
                            TipoVagaID = 1
                },
                new Vaga { VagaID = 2, 
                            Valor = 1000f, 
                            DataInicio = DateTime.Now, 
                            DataFim = DateTime.Now.AddDays(30), 
                            Requisitos = "Conhecimento em Java", 
                            Descricao = "Desenvolvimento de aplicação web", 
                            TituloProjeto = "Sistema de gerenciamento de vendas", 
                            Status = "Aberta", 
                            ProfessorID = 2, 
                            TipoVagaID = 1
                },
                new Vaga { VagaID = 3, 
                            Valor = 1000f, 
                            DataInicio = DateTime.Now, 
                            DataFim = DateTime.Now.AddDays(30), 
                            Requisitos = "Conhecimento em Java", 
                            Descricao = "Desenvolvimento de aplicação web", 
                            TituloProjeto = "Sistema de gerenciamento de vendas", 
                            Status = "Aberta", 
                            ProfessorID = 3, 
                            TipoVagaID = 2
                }
            );
            # endregion
      }
}
