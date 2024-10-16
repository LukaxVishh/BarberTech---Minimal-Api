using System.Text.Json.Serialization;

namespace MinimalApi.Agendas{
    public class AgendaDto
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string ClienteNome { get; set; }
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public required DateOnly DataAgendamento { get; init; }

    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public required TimeOnly HoraAgendamento { get; init; }
    public bool ServicoConcluido { get; set; }
}
}