namespace Domain.Products.IntegrationEvents;

public sealed record ProductChangedIntegrationEvent(
    int ProductId,
    ProductOperation Operation,
    long Version,
    DateTimeOffset OccurredAt,
    ProductSnapshot? Before,
    ProductSnapshot? After) : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
}
