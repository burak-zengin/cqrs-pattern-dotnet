using Domain.Products.ReadModels;

namespace Domain.Products.Repositories;

public interface IProductProjectionRepository
{
    Task UpsertAsync(ProductReadModel readModel, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);

    Task<long?> GetCurrentVersionAsync(int id, CancellationToken cancellationToken);
}
