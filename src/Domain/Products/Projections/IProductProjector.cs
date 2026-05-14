using Domain.Products.IntegrationEvents;
using Domain.Products.ReadModels;

namespace Domain.Products.Projections;

public interface IProductProjector
{
    ProductReadModel Project(ProductChangedIntegrationEvent @event);
}
