using Domain.Products.IntegrationEvents;

namespace Consumer.Infrastructure.Messaging;

public static class DebeziumEnvelopeMapper
{
    public static ProductChangedIntegrationEvent? Map(DebeziumEnvelope<DebeziumProduct>? envelope)
    {
        if (envelope?.Payload is null)
        {
            return null;
        }

        var payload = envelope.Payload;
        var operation = MapOperation(payload.Op);
        var id = (payload.After ?? payload.Before)?.Id;
        if (id is null)
        {
            return null;
        }

        var occurredAt = payload.TsMs.HasValue
            ? DateTimeOffset.FromUnixTimeMilliseconds(payload.TsMs.Value)
            : DateTimeOffset.UtcNow;

        var version = payload.Source?.Lsn
            ?? payload.Source?.TsMs
            ?? payload.TsMs
            ?? occurredAt.ToUnixTimeMilliseconds();

        return new ProductChangedIntegrationEvent(
            ProductId: id.Value,
            Operation: operation,
            Version: version,
            OccurredAt: occurredAt,
            Before: ToSnapshot(payload.Before),
            After: ToSnapshot(payload.After));
    }

    private static ProductOperation MapOperation(string? op) => op switch
    {
        "c" => ProductOperation.Created,
        "u" => ProductOperation.Updated,
        "d" => ProductOperation.Deleted,
        "r" => ProductOperation.Snapshot,
        _   => ProductOperation.Unknown
    };

    private static ProductSnapshot? ToSnapshot(DebeziumProduct? source) =>
        source is null
            ? null
            : new ProductSnapshot(source.Id, source.Name, source.Barcode, source.Color, source.Size);
}
