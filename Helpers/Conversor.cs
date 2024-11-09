using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string DateFormatBrazilian = "dd/MM/yyyy";
    private const string DateFormatIso = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        
        if (DateOnly.TryParseExact(dateString, DateFormatBrazilian, out var resultBrazilian))
        {
            return resultBrazilian;
        }
        
        if (DateOnly.TryParseExact(dateString, DateFormatIso, out var resultIso))
        {
            return resultIso;
        }
        
        throw new FormatException($"Invalid date format: '{dateString}'. Expected format is either '{DateFormatBrazilian}' or '{DateFormatIso}'.");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateFormatBrazilian));  // Exibindo sempre no formato brasileiro
    }
}

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string TimeFormat = "HH:mm:ss"; // Formato de tempo esperado

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var timeString = reader.GetString();

        // Utilizando TryParseExact com apenas os parâmetros necessários
        if (TimeOnly.TryParseExact(timeString, TimeFormat, out var result))
        {
            return result;
        }

        throw new FormatException($"Invalid time format: '{timeString}'. Expected format is '{TimeFormat}'.");
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(TimeFormat));
    }
}
