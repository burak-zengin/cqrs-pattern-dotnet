namespace Domain.Products.IntegrationEvents;

public sealed record ProductSnapshot(
    int Id,
    string Name,
    string Barcode,
    string Color,
    string Size);
