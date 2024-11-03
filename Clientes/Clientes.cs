namespace MinimalApi.Clientes
{
    public class Cliente
    {
        public Guid Id { get; init; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; } // Adicionando o campo Email

        public Cliente(string nome, string telefone, string senha)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Telefone = telefone;
            Senha = senha;
        }
    }
}
