# BarberTech API

Uma API minimalista desenvolvida em C# para gerenciar barbeiros e clientes. Permite o cadastro, consulta, atualização e exclusão de barbeiros e clientes, além de autenticação usando JWT.

## Funcionalidades

- **Gerenciamento de Barbeiros**
  - Criar, ler, atualizar e deletar barbeiros.
- **Gerenciamento de Clientes**
  - Criar, ler, atualizar e deletar clientes.
- **Autenticação**
  - Sistema de login com geração de token JWT.

## Tecnologias Utilizadas

- .NET 8.0
- Entity Framework Core
- BCrypt.Net para criptografia de senhas
- Swagger para documentação da API

## Instalação

### Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- Banco de dados (SQLite configurado)

### Passos para configuração

1. Clone o repositório:
   ```bash
   git clone https://github.com/LukaxVishh/BarberTech---Minimal-Api.git
   cd seurepositorio
Instale as dependências:

bash
dotnet restore
Crie o banco de dados e aplique as migrações:

bash
dotnet ef database update
Execute a aplicação:

bash
dotnet run
Acesse a API através de:

HTTP: http://localhost:5401
HTTPS: https://localhost:5400

Uso

Endpoints
Autenticação

POST /login
Body:
  json
  {
    "Username": "seu_usuario",
    "Password": "sua_senha"
  }
Retorna um token JWT se as credenciais forem válidas.

Barbeiros
POST /barbeiro

Cria um novo barbeiro.
GET /barbeiro

Retorna a lista de barbeiros cadastrados.
GET /barbeiro/{nome}

Retorna um barbeiro específico pelo nome.
PUT /barbeiro/{nome}

Atualiza os dados de um barbeiro.
DELETE /barbeiro/{nome}

Deleta um barbeiro específico.

Clientes
POST /cliente

Cria um novo cliente.
GET /cliente

Retorna a lista de clientes cadastrados.
GET /cliente/{nome}

Retorna um cliente específico pelo nome.
PUT /cliente/{nome}

Atualiza os dados de um cliente.
DELETE /cliente/{nome}

Deleta um cliente específico.

Documentação
A API está documentada com Swagger. Acesse /swagger no seu navegador para ver a documentação interativa.

Configuração
As configurações do JWT e outros parâmetros podem ser encontrados em appsettings.json:

json
  "Jwt": {
    "Key": "123456789",
    "Issuer": "BarberTechAPI",
    "Audience": "BarberTechApp",
    "ExpireMinutes": 60
  }

Contribuição
Sinta-se à vontade para abrir issues e pull requests!

Licença
Este projeto é licenciado sob a MIT License - veja o arquivo LICENSE para detalhes.
