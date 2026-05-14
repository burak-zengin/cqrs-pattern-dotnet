using Domain.Products.IntegrationEvents;
using Domain.Products.Projections;
using Domain.Products.ReadModels;

namespace Consumer.Infrastructure.Projections;

public sealed class ProductProjector : IProductProjector
{
    public ProductReadModel Project(ProductChangedIntegrationEvent @event)
    {
        var snapshot = @event.After
            ?? throw new InvalidOperationException(
                $"Cannot project a {@event.Operation} event without an 'after' snapshot.");

        return new ProductReadModel
        {
            Id = snapshot.Id,
            Name = snapshot.Name,
            Barcode = snapshot.Barcode,
            Color = snapshot.Color,
            Size = snapshot.Size,
            DisplayName = $"{snapshot.Name} - {snapshot.Color} / {snapshot.Size}",
            SearchText = $"{snapshot.Name} {snapshot.Barcode} {snapshot.Color} {snapshot.Size}".ToLowerInvariant(),
            Tags = new[]
            {
                snapshot.Color.ToLowerInvariant(),
                snapshot.Size.ToLowerInvariant()
            },
            Version = @event.Version,
            ProjectedAt = DateTimeOffset.UtcNow,
            SourceOperation = @event.Operation.ToString()
        };
    }
}
