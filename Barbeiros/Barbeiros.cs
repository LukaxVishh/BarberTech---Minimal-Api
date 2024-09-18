namespace MinimalApi.Barbeiros
{
    public class Barbeiro
    {
        public Guid Id { get; init; }
        public string Nome { get; private set; }
        public string Senha { get; private set; }
        public string Especialidade { get; private set; }

        public Barbeiro(string nome, string especialidade, string senha)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Especialidade = especialidade;
            Senha = senha;
        }

        public void AtualizarNome(string nome) { }
    }
}
