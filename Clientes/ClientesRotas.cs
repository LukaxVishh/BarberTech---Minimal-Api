using Microsoft.EntityFrameworkCore;
using MinimalApi.Data;

namespace MinimalApi.Clientes
{
    public static class ClientesRotas
    {
        public static void AddRotasClientes(this WebApplication app)
        {
            // Var que contém o pacote de rotas
            var Rotas = app.MapGroup("/cliente");

            Rotas.MapPost("", async (Cliente c, AppDbContext context) =>
            {
                if (await context.Clientes.AnyAsync(cliente => cliente.Nome == c.Nome)){
                    return Results.Conflict("Cliente já cadastrado");
                }

                var senhaEncriptada = BCrypt.Net.BCrypt.HashPassword(c.Senha);
                var novoCliente = new Cliente(c.Nome, c.Telefone, senhaEncriptada, c.Email);

                await context.Clientes.AddAsync(novoCliente);
                await context.SaveChangesAsync();

                return Results.Ok(novoCliente);
            }).WithTags("Cliente");

            Rotas.MapGet("", (AppDbContext context) => 
            {
                var clientes = context.Clientes.Select(cliente => new ClienteDTO(cliente.Id, cliente.Nome, cliente.Telefone, cliente.Email));

                return clientes;
            }).WithTags("Cliente");

            Rotas.MapGet("{nome}", async (string nome, AppDbContext context) => 
            {
                var tempCliente = await context.Clientes.SingleOrDefaultAsync(cliente => cliente.Nome == nome);
                
                if (tempCliente == null)
                {
                    return Results.NotFound("Usuário não encontrado.");
                }
                
                var cliente = new ClienteDTO(tempCliente.Id, tempCliente.Nome, tempCliente.Telefone, tempCliente.Email);

                return Results.Ok(cliente);
            }).WithTags("Cliente");

            Rotas.MapPut("{nome}", async(string nome, Cliente c, AppDbContext context) => 
            {
                var cliente = await context.Clientes.SingleOrDefaultAsync(cliente => cliente.Nome == nome);

                if (cliente == null)
                {
                    return Results.NotFound("Usuário não encontrado");
                }

                var senhaEncriptada = BCrypt.Net.BCrypt.HashPassword(c.Senha);
                cliente.Nome = c.Nome;
                cliente.Telefone = c.Telefone;
                cliente.Senha = senhaEncriptada;

                context.SaveChanges();
                return Results.Ok("Usuário atualizado!");
            }).WithTags("Cliente");

            Rotas.MapDelete("{nome}", (string nome, AppDbContext context) =>
            {
                var cliente = context.Clientes.SingleOrDefault(cliente => cliente.Nome == nome);

                if (cliente == null)
                {
                    return Results.NotFound("Usuário não encontrado");
                }

                context.Clientes.Remove(cliente);
                context.SaveChanges();
                return Results.Ok("Usuário deletado!");
            }).WithTags("Cliente");
        }
    }
}