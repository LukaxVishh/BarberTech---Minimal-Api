namespace MinimalApi.Agendas;

public class Agenda{

    public Guid Id { get; set; }
    public required string Nome { get; set; } 
    public required string ClienteNome { get; set; } 
    public DateOnly DataAgendamento { get; set; }
    public TimeOnly HoraAgendamento { get; set; }
    public bool ServicoConcluido { get; set; }
}
