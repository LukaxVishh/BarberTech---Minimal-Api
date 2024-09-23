namespace MinimalApi.Agendas{
    public class AgendaDto
{
    public Guid AgendamentoId { get; set; }
    public required string Nome { get; set; }
    public required string ClienteNome { get; set; }
    public DateTime DataAgendamento { get; set; }
    public TimeOnly HoraAgendamento { get; set; }
    public bool ServicoConcluido { get; set; }
}
}