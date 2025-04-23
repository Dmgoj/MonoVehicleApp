using System.ComponentModel.DataAnnotations;

public record VehicleRegistrationForCreateDto
{
    [Required, MaxLength(10)]
    public string RegistrationNumber { get; init; }

    [Required]
    public int ModelId { get; init; }

    [Required]
    public int EngineTypeId { get; init; }

    [Required]
    public int OwnerId { get; init; }
}