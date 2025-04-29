using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Project.DAL.Entities;

namespace Project.Service.Profiles
{
    public class VehicleModelProfile : Profile
    {
        public VehicleModelProfile()
        {
            CreateMap<VehicleModel, VehicleModelDto>();
            CreateMap<VehicleModelForCreateDto, VehicleModel>();
            CreateMap<VehicleModelForUpdateDto, VehicleModel>()
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
