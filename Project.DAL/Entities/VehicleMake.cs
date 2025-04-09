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
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(5)]
        public string Abrv { get; set; }
        public ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();
    }
}
