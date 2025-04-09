using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Common
{
    public interface IVehicleRegistration
    {
        int Id { get; set; }
        string RegistrationNumber { get; set; }
    }
}
