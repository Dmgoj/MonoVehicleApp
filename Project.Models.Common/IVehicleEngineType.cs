using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Common
{
    public interface IVehicleEngineType
    {
        int Id { get; set; }
        string Type { get; set; }
        string Abrv { get; set; }
    }
}
