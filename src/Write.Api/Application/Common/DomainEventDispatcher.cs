using Domain.Products.Events;
using MediatR;

namespace Write.Api.Application.Common;

public static class DomainEventDispatcher
{
    public static async Task PublishAllAsync(
        IPublisher publisher,
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken)
    {
        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = (INotification)Activator.CreateInstance(notificationType, domainEvent)!;
            await publisher.Publish(notification, cancellationToken);
        }
    }
}
