using BarberTech.MinimalApi.Autenticacao;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalApi.Autenticacao
{
    public static class AutenticacaoRota
    {
        public static void AddRotasAutenticacao(this WebApplication app)
        {
            app.MapPost("/login", (LoginDto loginDto, JwtTokenService jwtTokenService) =>
            {
            
                if (loginDto.Username == "admin" && loginDto.Password == "admin123") 
                {
                    var token = jwtTokenService.GenerateToken(loginDto.Username);
                    return Results.Ok(new { Token = token });
                }

                return Results.Unauthorized();
            }).WithTags("Autenticação");
        }
    }
}
