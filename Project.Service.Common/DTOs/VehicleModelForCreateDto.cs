using System.ComponentModel.DataAnnotations;

public record VehicleModelForCreateDto
{
    [Required, MaxLength(50)]
    public string Name { get; init; }

    [MaxLength(5)]
    public string Abrv { get; init; }

    [Required]
    public int VehicleMakeId { get; init; }
}