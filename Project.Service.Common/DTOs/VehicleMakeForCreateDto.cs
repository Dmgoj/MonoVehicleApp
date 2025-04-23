using System.ComponentModel.DataAnnotations;

public record VehicleMakeForCreateDto
{
    [Required, MaxLength(50)]
    public string Name { get; init; }

    [MaxLength(5)]
    public string Abrv { get; init; }
}