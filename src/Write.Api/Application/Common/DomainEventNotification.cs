using Domain.Products.Events;
using MediatR;

namespace Write.Api.Application.Common;

public sealed record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent) : INotification
    where TDomainEvent : IDomainEvent;
