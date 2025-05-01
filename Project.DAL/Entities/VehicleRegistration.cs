using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Project.DAL.Entities
{
    [Index(nameof(RegistrationNumber),IsUnique = true)]
    public class VehicleRegistration
    {
        public int Id { get; set; }
        [Required, MaxLength(10)]
        public string RegistrationNumber { get; set; }
        [Required]
        public int VehicleModelId { get; set; }
        [Required]
        public int VehicleEngineTypeId { get; set; }
        [Required]
        public int VehicleOwnerId { get; set; }
        public VehicleModel VehicleModel { get; set; }
        public VehicleEngineType VehicleEngineType { get; set; }
        public VehicleOwner VehicleOwner { get; set; }
    }
}
