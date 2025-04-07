using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Entities
{
    public class VehicleModel
    {
        int Id { get; set; }
        [Required, MaxLength(50)]
        string Name { get; set; }
        [MaxLength(5)]
        string Abrv { get; set; }
        [Required]
        int VehicleMakeId { get; set; }
        VehicleMake VehicleMake { get; set; }
        ICollection<VehicleRegistration> VehicleRegistrations { get; set; } = new List<VehicleRegistration>();
    }
}
