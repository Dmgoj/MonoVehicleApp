using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Project.DAL.Entities;

namespace Project.Service.Profiles
{
    internal class VehicleMakeProfile : Profile
    {
        public VehicleMakeProfile()
        {
            CreateMap<VehicleMake, VehicleMakeDto>();
            CreateMap<VehicleMakeForCreateDto, VehicleMake>();
            CreateMap<VehicleMakeForUpdateDto, VehicleMake>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                {
                    if (srcMember is string str)
                    {
                        return !string.IsNullOrWhiteSpace(str);
                    }
                    return srcMember != null;
                }));
        }
    }
}
