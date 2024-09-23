namespace MinimalApi.Agendas{
    
    public record AgendaRequest
{
    public required string Nome { get; init; }
    public required string ClienteNome { get; init; }
    public required DateTime DataAgendamento { get; init; }
    public required TimeOnly HoraAgendamento { get; init; }
    public required bool ServicoConcluido { get; init; }
}
}