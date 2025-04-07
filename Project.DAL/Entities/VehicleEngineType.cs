using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Entities
{
    public class VehicleEngineType
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(5)]
        public string Abrv { get; set; }
        public ICollection<VehicleRegistration> VehicleRegistrations { get; set; } = new List<VehicleRegistration>();
    }
}
