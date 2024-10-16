using System.Text.Json.Serialization;

namespace MinimalApi.Agendas{
    
    public record AgendaRequest
{
    public required string Nome { get; init; }

    public required string ClienteNome { get; init; }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public required DateOnly DataAgendamento { get; init; }

    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public required TimeOnly HoraAgendamento { get; init; }

    public required bool ServicoConcluido { get; init; }
}
}