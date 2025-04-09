using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models.Common;

namespace Project.Model
{
    public class VehicleEngineType : IVehicleEngineType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Abrv { get; set; }
    }
}
