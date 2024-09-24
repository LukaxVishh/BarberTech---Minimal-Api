using BarberTech.MinimalApi.Autenticacao;
using MinimalApi.Autenticacao;
using MinimalApi.Barbeiros;
using MinimalApi.Clientes;
using MinimalApi.Data;
using MinimalApi.Servicos;

var builder = WebApplication.CreateBuilder(args);

// Defina a URL e a porta aqui
builder.WebHost.UseUrls("https://localhost:5400");

// Configurar a autenticação JWT (método extraído para outro arquivo)
builder.Services.AddJwtAuthentication(builder.Configuration);

// Adicionar o serviço de autorização
builder.Services.AddAuthorization();
builder.Services.AddSingleton<JwtTokenService>();

// Registrar serviços adicionais
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppDbContext>();

var app = builder.Build();

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
app.AddRotasAgenda();
app.AddRotasServicos();
app.AddRotasAutenticacao();

app.Run();
