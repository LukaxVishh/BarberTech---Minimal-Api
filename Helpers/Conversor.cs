using Microsoft.VisualBasic;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string DateFormat = "dd/MM/yyyy";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        try
        {
            return DateOnly.ParseExact(dateString!, DateFormat);
        }
        catch (FormatException)
        {
            throw new JsonException($"Formato de data inválido. Use o formato {DateFormat}.");
        }
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateFormat));
    }
}

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string TimeFormat = "HH:mm";

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var timeString = reader.GetString();
        try
        {
            return TimeOnly.ParseExact(timeString!, TimeFormat);
        }
        catch (FormatException)
        {
            throw new JsonException($"Formato de hora inválido. Use o formato {TimeFormat}.");
        }
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(TimeFormat));
    }
}
