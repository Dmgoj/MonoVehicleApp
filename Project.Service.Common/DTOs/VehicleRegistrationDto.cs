public record VehicleRegistrationDto
{
    public int Id { get; init; }
    public string RegistrationNumber { get; init; }
    public int ModelId { get; init; }
    public string ModelName { get; init; }
    public int EngineTypeId { get; init; }
    public string EngineType { get; init; }
    public int OwnerId { get; init; }
    public string OwnerFullName { get; init; }
}