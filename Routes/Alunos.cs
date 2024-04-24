using connect_cic_api.Domain;
using connect_cic_api.Infra.Persistence;

namespace connect_cic_api.API.Endpoints;

public static class Alunos
{
    public static void RegisterAlunosEndpoint(this IEndpointRouteBuilder routes){
        
        var AlunosRoutes = routes.MapGroup("/vagas/{id}/Alunos");
        //Gets
        //  /alunos - lista alunos  ok
        AlunosRoutes.MapGet("", async (ConnectCICAPIContext context) =>
        {
            
            return await context.Alunos.ToListAsync();
        
        });
        //  /alunos/id - um aluno especifico
        AlunosRoutes.MapGet("", (ConnectCICAPIContext context) => context.Alunos.ToList());

        // Posts
        // /alunos - cadastra aluno ok
        AlunosRoutes.MapPost("", async (Aluno aluno, ConnectCICAPIContext context) =>
        {
            context.Alunos.Add(aluno);
            await context.SaveChangesAsync();
            return aluno;
        });

        // /vagas/id/alunos - aluno se candidata a vaga
        AlunosRoutes.MapPost("", (Aluno alunosrout, ConnectCICAPIContext context) =>
        {
            context.Alunos.Add(alunosrout);
            
            context.SaveChanges();
            
            return alunosrout;
        });
    

        // Puts
        // /alunos/id - atualiza aluno ok
        AlunosRoutes.MapPut("/{id}", async (int id, Aluno aluno, ConnectCICAPIContext context) =>
        {
            var alunoToUpdate = await context.Alunos.FirstOrDefaultAsync(c => c.AlunoID == id);
            
            if (alunoToUpdate != null)
            {
                alunoToUpdate.Nome = aluno.Nome;
                await context.SaveChangesAsync();
            }
            
            return alunoToUpdate;
        });
        
        AlunosRoutes.MapPut("/{id}", (int id, Aluno alunosrout, ConnectCICAPIContext context) => 
        {
         
            var AlunosToUpdate = context.Alunos.FirstOrDefault(c => c.AlunoID == id);

            if(AlunosToUpdate is not null){
            
                AlunosToUpdate.Nome = alunosrout.Nome; // editar o if
            
                context.SaveChanges();
            }

            return AlunosToUpdate;
        });

        // Deletes
        // /alunos/id - deleta aluno ok
        AlunosRoutes.MapDelete("/{id}", async (int id, ConnectCICAPIContext context) =>
        {
        
            var alunoToDelete = await context.Alunos.FirstOrDefaultAsync(c => c.AlunoID == id);
        
            if (alunoToDelete != null)
            {
            
                context.Alunos.Remove(alunoToDelete);
            
                await context.SaveChangesAsync();
            
            }
        
            return alunoToDelete;
        
        });

        // /vagas/idVaga/alunos/idAluno - aluno retira interesse na vaga

        AlunosRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>
        {
            var AlunosToDelete = context.Alunos.FirstOrDefault(c => c.AlunoID == id);
        
            context.Alunos.Remove(AlunosToDelete);
        
            context.SaveChanges();
        
            return AlunosToDelete;
        });

    }
}

 