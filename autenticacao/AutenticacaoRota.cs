using BarberTech.MinimalApi.Autenticacao;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Data;
using MinimalApi.Clientes;

namespace MinimalApi.Autenticacao
{
    public static class AutenticacaoRota
    {
        public static void AddRotasAutenticacao(this WebApplication app)
        {
            // Rota para login
            app.MapPost("/login", (LoginDto loginDto, JwtTokenService jwtTokenService) =>
            {
                if (loginDto.Username == "admin" && loginDto.Password == "admin123") 
                {
                    var token = jwtTokenService.GenerateToken(loginDto.Username);
                    return Results.Ok(new { Token = token });
                }
                
                if (loginDto.Username == "barbeiro" && loginDto.Password == "barbeiro123")
                {
                    var token = jwtTokenService.GenerateToken(loginDto.Username);
                    return Results.Ok(new { Token = token });
                }

                return Results.Unauthorized();
            }).WithTags("Autenticação");

            // Rota para registro
            app.MapPost("/register", async (RegisterDto registerDto, AppDbContext dbContext) =>
            {
                // Verifica se o telefone ou email já estão em uso
                var existingCliente = await dbContext.Set<Cliente>()
                    .FirstOrDefaultAsync(c => c.Telefone == registerDto.Telefone || c.Email == registerDto.Email);

                if (existingCliente != null)
                {
                    return Results.BadRequest("Telefone ou email já em uso.");
                }

                // Cria um novo cliente
                var newCliente = new Cliente(registerDto.Nome, registerDto.Telefone, registerDto.Senha, registerDto.Email)
                {
                    Email = registerDto.Email
                };

                dbContext.Clientes.Add(newCliente);
                await dbContext.SaveChangesAsync();

                return Results.Ok("Conta criada com sucesso!");
            }).WithTags("Autenticação");
        }
    }
}
