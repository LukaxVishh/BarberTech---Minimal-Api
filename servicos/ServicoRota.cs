using Microsoft.EntityFrameworkCore;
using MinimalApi.Data;

namespace MinimalApi.Servicos
{
    public static class ServicoRota
    {
        public static void AddRotasServicos(this WebApplication app)
        {
            var rotasServico = app.MapGroup("/servico");

            // ROTA PARA CRIAÇÃO DE NOVO SERVIÇO
            rotasServico.MapPost("", async (ServicoRequest request, AppDbContext context) =>
            {
                if (string.IsNullOrEmpty(request.Nome) || request.Preco <= 0)
                {
                    return Results.BadRequest("Nome e Preço são obrigatórios");
                }

                var servicoExistente = await context.Servicos.AnyAsync(servico => servico.Nome == request.Nome);

                if (servicoExistente)
                {
                    return Results.Conflict("Serviço já existente");
                }

                var novoServico = new Servico(request.Nome, request.Preco);
                await context.Servicos.AddAsync(novoServico);
                await context.SaveChangesAsync();

                var servicoRetorno = new ServicoDto(novoServico.Id, novoServico.Nome, novoServico.Preco);

                return Results.Ok(servicoRetorno);
            }).WithTags("Serviço");

            // ROTAS PARA RETORNAR LISTA DE SERVIÇOS CADASTRADOS OU UM SERVIÇO ESPECÍFICO
            rotasServico.MapGet("", (AppDbContext context) =>
            {
                var servicos = context.Servicos.Select(servico => new ServicoDto(servico.Id, servico.Nome, servico.Preco));
                return Results.Ok(servicos);
            }).WithTags("Serviço");

            rotasServico.MapGet("{nome}", (string nome, AppDbContext context) =>
            {
                var servico = context.Servicos.Where(servico => servico.Nome == nome).Select(servico => new ServicoDto(servico.Id, servico.Nome, servico.Preco));

                if (!servico.Any())
                {
                    return Results.NotFound("Serviço não encontrado");
                }

                return Results.Ok(servico);
            }).WithTags("Serviço");

            // ROTA PARA ATUALIZAR DADOS DE UM SERVIÇO ESPECÍFICO
            rotasServico.MapPut("{nome}", async (string nome, ServicoRequest request, AppDbContext context) =>
            {
                var servico = await context.Servicos.SingleOrDefaultAsync(servico => servico.Nome == nome);

                if (servico == null)
                {
                    return Results.NotFound("Serviço não encontrado");
                }

                servico.AtualizarNome(request.Nome);
                servico.AtualizarPreco(request.Preco);
                await context.SaveChangesAsync();

                return Results.Ok(servico);
            }).WithTags("Serviço");

            // ROTA PARA DELETAR UM SERVIÇO ESPECÍFICO
            rotasServico.MapDelete("{nome}", async (string nome, AppDbContext context) =>
            {
                var servico = await context.Servicos.SingleOrDefaultAsync(servico => servico.Nome == nome);

                if (servico == null)
                {
                    return Results.NotFound("Serviço não encontrado");
                }

                context.Servicos.Remove(servico);
                await context.SaveChangesAsync();
                return Results.Ok(servico);
            }).WithTags("Serviço");
        }
    }
}
