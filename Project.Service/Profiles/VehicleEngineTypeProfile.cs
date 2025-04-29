using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Project.DAL.Entities;

namespace Project.Service.Profiles
{
    public class VehicleEngineTypeProfile : Profile
    {
        public VehicleEngineTypeProfile()
        {
            CreateMap<VehicleEngineType, VehicleEngineTypeDto>();
        }
    }
}
