using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Entities
{
    public class VehicleMake
    {
        int Id { get; set; }
        [Required, MaxLength(50)]
        string Name { get; set; }
        [MaxLength(5)]
        string Abrv { get; set; }
        ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();
    }
}
