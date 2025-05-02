public record VehicleOwnerDto
{
    public int Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime DOB { get; init; }
    public int? VehicleModelId { get; init; }
    public string VehicleModel { get; init; }
    public int? VehicleMakeId { get; init; }
    public string VehicleMake { get; init; }
}