using Microsoft.EntityFrameworkCore;
using MinimalApi.Data;
using System;

namespace MinimalApi.Barbeiros
{
    public static class BarbeirosRota
    {
        public static void AddRotasBarbeiros(this WebApplication app)
        {
            var rotasBarbeiro = app.MapGroup("/barbeiro");

            rotasBarbeiro.MapPost("", async (BarbeiroRequests request, AppDbContext context) =>
            {
                var barbeiroExistente = await context.Barbeiros.AnyAsync(barbeiro => barbeiro.Nome == request.Nome);

                if (barbeiroExistente)
                {
                    return Results.Conflict("Usuário existente");
                }

                var novoBarbeiro = new Barbeiro(request.Nome, request.Especialidade, request.Senha);
                await context.Barbeiros.AddAsync(novoBarbeiro);
                await context.SaveChangesAsync();

                var barbeiroRetorno = new BarbeiroDto(novoBarbeiro.Id, novoBarbeiro.Nome, novoBarbeiro.Especialidade);

                return Results.Ok(barbeiroRetorno);
            });

            rotasBarbeiro.MapGet("", (AppDbContext context) =>
            {
                var barbeiros = context.Barbeiros.Select(barbeiro => new BarbeiroDto(barbeiro.Id, barbeiro.Nome, barbeiro.Especialidade));
                
                return barbeiros;
            });

            rotasBarbeiro.MapPut("{nome}", async (string nome, BarbeiroRequests request, AppDbContext context) =>
            {
                var barbeiro = await context.Barbeiros.SingleOrDefaultAsync(barbeiro => barbeiro.Nome == nome);

                if (barbeiro == null)
                {
                    return Results.NotFound();
                }

                barbeiro.AtualizarNome(request.Nome);
                await context.SaveChangesAsync();

                return Results.Ok(barbeiro);
            });

            rotasBarbeiro.MapDelete("{nome}", (string nome, AppDbContext context) =>
            {
                var barbeiro = context.Barbeiros.SingleOrDefault(barbeiro => barbeiro.Nome == nome);

                if (barbeiro == null)
                {
                    return Results.NotFound();
                }

                context.Barbeiros.Remove(barbeiro);
                context.SaveChanges();
                return Results.Ok(barbeiro);
            });
        }
    }
}
