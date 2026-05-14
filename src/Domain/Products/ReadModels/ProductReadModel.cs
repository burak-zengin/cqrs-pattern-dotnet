namespace Domain.Products.ReadModels;

public sealed class ProductReadModel
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public string Barcode { get; init; } = default!;

    public string Color { get; init; } = default!;

    public string Size { get; init; } = default!;

    public string DisplayName { get; init; } = default!;

    public string SearchText { get; init; } = default!;

    public string[] Tags { get; init; } = Array.Empty<string>();

    public long Version { get; init; }

    public DateTimeOffset ProjectedAt { get; init; }

    public string SourceOperation { get; init; } = default!;
}
