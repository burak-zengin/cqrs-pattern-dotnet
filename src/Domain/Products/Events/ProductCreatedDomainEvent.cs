namespace Domain.Products.Events;

public sealed record ProductCreatedDomainEvent(int ProductId, string Barcode) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();

    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}
