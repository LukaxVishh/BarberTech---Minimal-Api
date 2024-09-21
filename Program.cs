using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.Barbeiros;
using MinimalApi.Clientes;
using MinimalApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Defina a URL e a porta aqui
builder.WebHost.UseUrls("https://localhost:5400");

// Adicionar o serviço de autenticação JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero 
    };
});

// Adicionar o serviço de autorização
builder.Services.AddAuthorization();

// Registrar serviços adicionais
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppDbContext>();

var app = builder.Build();

// Endpoint de login para gerar o token JWT
app.MapPost("/login", (loginDTO login, IConfiguration configuration) =>
{
    // Valida credenciais (exemplo estático)
    if (login.Username == "admin" && login.Password == "admin123") // Aqui você pode validar no banco de dados
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, login.Username)
            }),
            Expires = DateTime.UtcNow.AddMinutes(60), // Expiração do token
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        // Retorna o token JWT
        return Results.Ok(new { Token = tokenString });
    }

    // Retorna não autorizado se as credenciais estiverem incorretas
    return Results.Unauthorized();
});

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ativar autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Rotas adicionais
app.AddRotasBarbeiros();
app.AddRotasClientes();

app.Run();