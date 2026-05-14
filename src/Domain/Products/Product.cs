using Domain.Products.Events;
using Domain.Products.ValueObjects;

namespace Domain.Products;

public sealed class Product
{
    private readonly List<IDomainEvent> _domainEvents = new();

    private Product() { }

    public int Id { get; private set; }

    public string Name { get; private set; } = default!;

    public Barcode Barcode { get; private set; }

    public string Color { get; private set; } = default!;

    public string Size { get; private set; } = default!;

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public static Product Create(string name, string barcode, string color, string size)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(color))
        {
            throw new ArgumentException("Color is required.", nameof(color));
        }

        if (string.IsNullOrWhiteSpace(size))
        {
            throw new ArgumentException("Size is required.", nameof(size));
        }

        return new Product
        {
            Name = name.Trim(),
            Barcode = Barcode.Create(barcode),
            Color = color.Trim(),
            Size = size.Trim()
        };
    }

    public void MarkCreated()
    {
        _domainEvents.Add(new ProductCreatedDomainEvent(Id, Barcode.Value));
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
