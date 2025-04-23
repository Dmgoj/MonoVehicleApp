using System.ComponentModel.DataAnnotations;

public record VehicleOwnerForUpdateDto
{
    [Required, MaxLength(100)]
    public string FirstName { get; init; }

    [Required, MaxLength(100)]
    public string LastName { get; init; }

    [Required]
    public DateTime DOB { get; init; }
}
