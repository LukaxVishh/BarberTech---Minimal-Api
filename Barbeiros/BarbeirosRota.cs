using Microsoft.EntityFrameworkCore;
using MinimalApi.Data;


namespace MinimalApi.Barbeiros
{
    public static class BarbeirosRota
    {
        public static void AddRotasBarbeiros(this WebApplication app)
        {
            var rotasBarbeiro = app.MapGroup("/barbeiro");


            // ROTA PARA CRIAÇÃO DE NOVO BARBEIRO

            rotasBarbeiro.MapPost("", async (BarbeiroRequests request, AppDbContext context) =>
            {

                if (string.IsNullOrEmpty(request.Nome) || string.IsNullOrEmpty(request.Senha))
                {
                    return Results.BadRequest("Nome e Senha são obrigatórios");
                }
                
                var barbeiroExistente = await context.Barbeiros.AnyAsync(barbeiro => barbeiro.Nome == request.Nome);

                if (barbeiroExistente)
                {
                    return Results.Conflict("Usuário existente");
                }

                var senhaEncriptada = BCrypt.Net.BCrypt.HashPassword(request.Senha);
                var novoBarbeiro = new Barbeiro(request.Nome, request.Especialidade, senhaEncriptada);
                await context.Barbeiros.AddAsync(novoBarbeiro);
                await context.SaveChangesAsync();

                var barbeiroRetorno = new BarbeiroDto(novoBarbeiro.Id, novoBarbeiro.Nome, novoBarbeiro.Especialidade);

                return Results.Ok(barbeiroRetorno);
            }).WithTags("Barbeiro");


            // ROTAS PARA RETORNAR LISTA DE BARBEIROS CADASTRADOS OU BARBEIRO ESPECIFICO

            rotasBarbeiro.MapGet("", (AppDbContext context) =>
            {
                var barbeiros = context.Barbeiros.Select(barbeiro => new BarbeiroDto(barbeiro.Id, barbeiro.Nome, barbeiro.Especialidade));
                
                return barbeiros;
            }).WithTags("Barbeiro");

            rotasBarbeiro.MapGet("{nome}", (string nome, AppDbContext context) =>
            {
                var barbeiro = context.Barbeiros.Where(barbeiro => barbeiro.Nome == nome).Select(barbeiro => new BarbeiroDto(barbeiro.Id, barbeiro.Nome, barbeiro.Especialidade));

                if (barbeiro == null)
                {
                    return Results.NotFound("Usuário não encontrado");
                }

                return Results.Ok(barbeiro);
            }).WithTags("Barbeiro");


            // ROTA PARA ATUALIZAR DADOS DE UM BARBEIRO ESPECIFICO

           rotasBarbeiro.MapPut("{nome}", async (string nome, BarbeiroRequests request, AppDbContext context) =>
            {
                var barbeiro = await context.Barbeiros.SingleOrDefaultAsync(barbeiro => barbeiro.Nome == nome);

                if (barbeiro == null)
                {
                    return Results.NotFound("Usuário não encontrado");
                }

                // Atualiza o nome, se fornecido
                if (!string.IsNullOrEmpty(request.Nome))
                {
                    barbeiro.AtualizarNome(request.Nome); // Atualizando o nome com o método
                }

                // Atualiza a especialidade, se fornecido
                if (!string.IsNullOrEmpty(request.Especialidade))
                {
                    barbeiro.AtualizarEspecialidade(request.Especialidade); // Atualizando a especialidade com o método
                }

                // Atualiza a senha, se fornecida
                if (!string.IsNullOrEmpty(request.Senha))
                {
                    var senhaEncriptada = BCrypt.Net.BCrypt.HashPassword(request.Senha);
                    barbeiro.AtualizarSenha(senhaEncriptada); // Atualizando a senha com o método
                }

                await context.SaveChangesAsync();

                return Results.Ok(new BarbeiroDto(barbeiro.Id, barbeiro.Nome, barbeiro.Especialidade));
            });



            // ROTA PARA DELETAR UM BARBEIRO ESPECIFICO

            rotasBarbeiro.MapDelete("{nome}", (string nome, AppDbContext context) =>
            {
                var barbeiro = context.Barbeiros.SingleOrDefault(barbeiro => barbeiro.Nome == nome);

                if (barbeiro == null)
                {
                    return Results.NotFound("Usuário não encontrado");
                }

                context.Barbeiros.Remove(barbeiro);
                context.SaveChanges();
                return Results.Ok(barbeiro);
            }).WithTags("Barbeiro");
        }
    }
}
