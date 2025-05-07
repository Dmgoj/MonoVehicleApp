using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Project.DAL.Entities;

namespace Project.Service.Profiles
{
    public class VehicleRegistrationProfile : Profile
    {
        public VehicleRegistrationProfile()
        {
            CreateMap<VehicleRegistration, CarDto>()
                .ForMember(d => d.Make, o => o.MapFrom(r => r.VehicleModel.VehicleMake.Name))
                .ForMember(d => d.Model, o => o.MapFrom(r => r.VehicleModel.Name))
                .ForMember(d => d.RegistrationNumber, o => o.MapFrom(r => r.RegistrationNumber));
            CreateMap<VehicleRegistrationForCreateDto, VehicleRegistration>();
            CreateMap<VehicleRegistrationForUpdateDto, VehicleRegistration>();
        }
    }
}
