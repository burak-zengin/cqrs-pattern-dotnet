namespace Domain.Products.IntegrationEvents;

public enum ProductOperation
{
    Unknown = 0,
    Created = 1,
    Updated = 2,
    Deleted = 3,
    Snapshot = 4
}
