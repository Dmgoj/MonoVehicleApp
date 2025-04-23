using System.ComponentModel.DataAnnotations;

public record VehicleMakeForUpdateDto
{
    [Required, MaxLength(50)]
    public string Name { get; init; }

    [MaxLength(5)]
    public string Abrv { get; init; }
}