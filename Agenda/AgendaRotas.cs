using Microsoft.EntityFrameworkCore;
using MinimalApi.Agendas;
using MinimalApi.Data;

public static class RotasAgendaExtensions
{
    public static void AddRotasAgenda(this WebApplication app)
    {
        var rotas = app.MapGroup("/agenda");

        // Rota para criar um novo agendamento
        rotas.MapPost("", async (AppDbContext db, AgendaRequest agendaRequest) =>
        {
            app.Logger.LogInformation("Processando criação de agendamento.");

            // Verificar se o barbeiro existe
            var barbeiro = await db.Barbeiros.SingleOrDefaultAsync(b => b.Nome == agendaRequest.Nome);
            if (barbeiro == null)
            {
                return Results.NotFound("Barbeiro não encontrado.");
            }

            // Verificar se o cliente existe
            var cliente = await db.Clientes.SingleOrDefaultAsync(c => c.Nome == agendaRequest.ClienteNome);
            if (cliente == null)
            {
                return Results.NotFound("Cliente não encontrado.");
            }

            // Verificar se a data e hora do agendamento são válidas
            if (agendaRequest.DataAgendamento < DateTime.Now)
            {
                return Results.BadRequest("A data do agendamento não pode ser no passado.");
            }

            // Verificar conflito de horário
            var conflito = await db.Agenda
                .AnyAsync(a => a.Nome == barbeiro.Nome
                            && a.DataAgendamento == agendaRequest.DataAgendamento
                            && a.HoraAgendamento == agendaRequest.HoraAgendamento);
            if (conflito)
            {
                return Results.Conflict("Já existe um agendamento para esse horário.");
            }

            // Criar novo agendamento
            var agendamento = new Agenda
            {
                Id = Guid.NewGuid(),
                Nome = barbeiro.Nome,
                ClienteNome = cliente.Nome,
                DataAgendamento = agendaRequest.DataAgendamento,
                HoraAgendamento = agendaRequest.HoraAgendamento,
                ServicoConcluido = agendaRequest.ServicoConcluido
            };

            db.Agenda.Add(agendamento);
            await db.SaveChangesAsync();

            app.Logger.LogInformation("Agendamento criado com sucesso.");
            return Results.Created($"/Agenda/{agendamento.Id}", new AgendaDto
            {
                Id = agendamento.Id,
                Nome = agendamento.Nome,
                ClienteNome = agendamento.ClienteNome,
                DataAgendamento = agendamento.DataAgendamento,
                HoraAgendamento = agendamento.HoraAgendamento,
                ServicoConcluido = agendamento.ServicoConcluido
            });
        }).WithTags("Agendamento");

        // Rota para listar todos os agendamentos
        rotas.MapGet("", async (AppDbContext db) =>
        {
            app.Logger.LogInformation("Listando todos os agendamentos.");
            var agendamentos = await db.Agenda.ToListAsync();
            var agendamentosDto = agendamentos.Select(a => new AgendaDto
            {
                Id = a.Id,
                Nome = a.Nome,
                ClienteNome = a.ClienteNome,
                DataAgendamento = a.DataAgendamento,
                HoraAgendamento = a.HoraAgendamento,
                ServicoConcluido = a.ServicoConcluido
            });
            return Results.Ok(agendamentosDto);
        }).WithTags("Agendamento");

        // Rota para atualizar um agendamento
        rotas.MapPut("/{id}", async (Guid id, AppDbContext db, AgendaRequest agendaRequest) =>
        {
            app.Logger.LogInformation($"Atualizando agendamento {id}.");

            var agendamento = await db.Agenda.FindAsync(id);
            if (agendamento == null)
            {
                return Results.NotFound("Agendamento não encontrado.");
            }

            // Verificar se o barbeiro existe
            var barbeiro = await db.Barbeiros.SingleOrDefaultAsync(b => b.Nome == agendaRequest.Nome);
            if (barbeiro == null)
            {
                return Results.NotFound("Barbeiro não encontrado.");
            }

            // Verificar se o cliente existe
            var cliente = await db.Clientes.SingleOrDefaultAsync(c => c.Nome == agendaRequest.ClienteNome);
            if (cliente == null)
            {
                return Results.NotFound("Cliente não encontrado.");
            }

            // Atualizar informações do agendamento
            agendamento.Nome = barbeiro.Nome;
            agendamento.ClienteNome = cliente.Nome;
            agendamento.DataAgendamento = agendaRequest.DataAgendamento;
            agendamento.HoraAgendamento = agendaRequest.HoraAgendamento;
            agendamento.ServicoConcluido = agendaRequest.ServicoConcluido;

            await db.SaveChangesAsync();
            return Results.Ok(agendamento);
        }).WithTags("Agendamento");

        // Rota para deletar um agendamento
        rotas.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
        {
            app.Logger.LogInformation($"Deletando agendamento {id}.");

            var agendamento = await db.Agenda.FindAsync(id);
            if (agendamento == null)
            {
                return Results.NotFound("Agendamento não encontrado.");
            }

            db.Agenda.Remove(agendamento);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Agendamento");
    }
}