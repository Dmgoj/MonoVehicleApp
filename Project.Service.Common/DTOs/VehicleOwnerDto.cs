public record VehicleOwnerDto
{
    public int Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime DOB { get; init; }
    public List<CarDto> Cars { get; init; }
}