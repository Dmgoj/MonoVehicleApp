using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Project.DAL.Entities;

namespace Project.Service.Profiles
{
    public class VehicleOwnerProfile : Profile
    {
        public VehicleOwnerProfile()
        {
            CreateMap<VehicleOwner, VehicleOwnerDto>()
                .ForMember(d => d.Cars, opt => opt.MapFrom(src => src.VehicleRegistrations));
            CreateMap<VehicleOwnerForCreateDto, VehicleOwner>();
            CreateMap<VehicleOwnerForUpdateDto, VehicleOwner>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                {
                    if (srcMember is string str)
                    {
                        return !string.IsNullOrWhiteSpace(str);
                    }
                    else if (srcMember is DateTime dateTime)
                    {
                        return dateTime != default;
                    }
                    return true;
                }));
        }
    }
}
