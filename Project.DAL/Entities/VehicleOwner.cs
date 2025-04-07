using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Entities
{
    public class VehicleOwner
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        ICollection<VehicleRegistration> VehicleRegistrations { get; set; } = new List<VehicleRegistration>();

    }
}
