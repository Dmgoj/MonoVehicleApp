public record VehicleModelDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Abrv { get; init; }
    public int VehicleMakeId { get; init; }
    public string VehicleMakeName { get; init; }
}