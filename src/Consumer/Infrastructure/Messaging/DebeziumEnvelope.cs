using System.Text.Json.Serialization;

namespace Consumer.Infrastructure.Messaging;

public sealed class DebeziumEnvelope<T>
{
    [JsonPropertyName("payload")]
    public DebeziumPayload<T>? Payload { get; set; }
}

public sealed class DebeziumPayload<T>
{
    [JsonPropertyName("before")]
    public T? Before { get; set; }

    [JsonPropertyName("after")]
    public T? After { get; set; }

    [JsonPropertyName("op")]
    public string? Op { get; set; }

    [JsonPropertyName("ts_ms")]
    public long? TsMs { get; set; }

    [JsonPropertyName("source")]
    public DebeziumSource? Source { get; set; }
}

public sealed class DebeziumSource
{
    [JsonPropertyName("lsn")]
    public long? Lsn { get; set; }

    [JsonPropertyName("ts_ms")]
    public long? TsMs { get; set; }
}

public sealed class DebeziumProduct
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("Barcode")]
    public string Barcode { get; set; } = default!;

    [JsonPropertyName("Color")]
    public string Color { get; set; } = default!;

    [JsonPropertyName("Size")]
    public string Size { get; set; } = default!;
}
