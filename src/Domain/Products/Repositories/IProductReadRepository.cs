using Domain.Products.ReadModels;

namespace Domain.Products.Repositories;

public interface IProductReadRepository
{
    Task<ProductReadModel?> GetAsync(int id, CancellationToken cancellationToken);

    Task<List<ProductReadModel>> GetAllAsync(CancellationToken cancellationToken);
}
