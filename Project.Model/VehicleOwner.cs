using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models.Common;

namespace Project.Model
{
    public class VehicleOwner : IVehicleOwner
    {
        public int Id { get; set; }
        public string FirstName { get ; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
    }
}
