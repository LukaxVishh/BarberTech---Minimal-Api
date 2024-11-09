using BarberTech.MinimalApi.Autenticacao;
using MinimalApi.Autenticacao;
using MinimalApi.Barbeiros;
using MinimalApi.Clientes;
using MinimalApi.Data;
using MinimalApi.Servicos;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:5400");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.AddRotasBarbeiros();
app.AddRotasClientes();
app.AddRotasAgenda();
app.AddRotasServicos();
app.AddRotasAutenticacao();

app.Run();
