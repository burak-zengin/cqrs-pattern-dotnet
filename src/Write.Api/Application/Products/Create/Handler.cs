using Domain.Products;
using Domain.Products.Repositories;
using MediatR;
using Write.Api.Application.Common;

namespace Write.Api.Application.Products.Create;

public class Handler(IProductWriteRepository repository, IPublisher publisher)
    : IRequestHandler<Command, int>
{
    public async Task<int> Handle(Command request, CancellationToken cancellationToken)
    {
        var product = Product.Create(request.Name, request.Barcode, request.Color, request.Size);

        var id = await repository.CreateAsync(product, cancellationToken);

        product.MarkCreated();
        await DomainEventDispatcher.PublishAllAsync(publisher, product.DomainEvents, cancellationToken);
        product.ClearDomainEvents();

        return id;
    }
}
