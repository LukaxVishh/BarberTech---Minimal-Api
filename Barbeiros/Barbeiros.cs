namespace MinimalApi.Barbeiros
{
    public class Barbeiro
    {
        public Guid Id { get; init; }
        public string Nome { get; private set; }
        public string Senha { get; private set; }
        public string Especialidade { get; private set; }

        // Construtor para criar um novo barbeiro
        public Barbeiro(string nome, string especialidade, string senha)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Especialidade = especialidade;
            Senha = senha;
        }

        // Método para atualizar o nome
        public void AtualizarNome(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException("Nome não pode ser vazio.");
            }
            Nome = nome; // Atualiza o nome diretamente
        }

        // Método para atualizar a especialidade
        public void AtualizarEspecialidade(string especialidade)
        {
            if (string.IsNullOrEmpty(especialidade))
            {
                throw new ArgumentException("Especialidade não pode ser vazia.");
            }
            Especialidade = especialidade; // Atualiza a especialidade
        }

        // Método para atualizar a senha
        public void AtualizarSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha))
            {
                throw new ArgumentException("Senha não pode ser vazia.");
            }
            Senha = senha; // Atualiza a senha
        }
    }
}
