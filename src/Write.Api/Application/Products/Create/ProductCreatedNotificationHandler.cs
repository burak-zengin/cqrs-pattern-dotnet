using Domain.Products.Events;
using MediatR;
using Write.Api.Application.Common;

namespace Write.Api.Application.Products.Create;

public sealed class ProductCreatedNotificationHandler(ILogger<ProductCreatedNotificationHandler> logger)
    : INotificationHandler<DomainEventNotification<ProductCreatedDomainEvent>>
{
    public Task Handle(
        DomainEventNotification<ProductCreatedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        var @event = notification.DomainEvent;
        logger.LogInformation(
            "Product created. Id={ProductId}, Barcode={Barcode}, OccurredAt={OccurredAt}, EventId={EventId}",
            @event.ProductId,
            @event.Barcode,
            @event.OccurredAt,
            @event.EventId);

        return Task.CompletedTask;
    }
}
