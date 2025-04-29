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
            CreateMap<VehicleRegistration, VehicleRegistrationDto>()
                 .ForMember(dest => dest.ModelName,
                           opt => opt.MapFrom(src => src.VehicleModel.Name))
                .ForMember(dest => dest.EngineType,
                           opt => opt.MapFrom(src => src.VehicleEngineType.Type))
                .ForMember(dest => dest.OwnerFullName,
                           opt => opt.MapFrom(src => $"{src.VehicleOwner.FirstName} {src.VehicleOwner.LastName}"));
            CreateMap<VehicleRegistrationForCreateDto, VehicleRegistration>();
            CreateMap<VehicleRegistrationForUpdateDto, VehicleRegistration>();
        }
    }
}
