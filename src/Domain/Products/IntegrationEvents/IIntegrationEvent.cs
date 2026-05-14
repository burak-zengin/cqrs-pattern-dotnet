namespace Domain.Products.IntegrationEvents;

public interface IIntegrationEvent
{
    Guid Id { get; }

    DateTimeOffset OccurredAt { get; }
}
