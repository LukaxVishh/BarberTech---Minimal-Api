using MinimalApi.Barbeiros;
using MinimalApi.Clientes;
using MinimalApi.Data;


var builder = WebApplication.CreateBuilder(args);

// Defina a URL e a porta aqui
builder.WebHost.UseUrls("https://localhost:5400");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddRotasBarbeiros();
app.AddRotasClientes();

app.Run();

