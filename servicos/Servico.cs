namespace MinimalApi.Servicos
{
    public class Servico
    {
        public Guid Id { get; init; }
        public string Nome { get; private set; }
        public decimal Preco { get; private set; }

        public Servico(string nome, decimal preco)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Preco = preco;
        }

        public void AtualizarNome(string nome)
        {
            Nome = nome;
        }

        public void AtualizarPreco(decimal preco)
        {
            Preco = preco;
        }
    }
}
