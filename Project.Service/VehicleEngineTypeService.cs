using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Project.DAL.Entities;
using Project.Repository.Common;
using Project.Service.Common;

namespace Project.Service
{
    public class VehicleEngineTypeService : IVehicleEngineTypeService
    {
        private readonly IRepository<VehicleEngineType> _repository;
        private readonly IMapper _mapper;

        public VehicleEngineTypeService(
            IRepository<VehicleEngineType> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VehicleEngineTypeDto>> GetAllAsync()
        {
            var entities = await _repository.Get();
            return _mapper.Map<IEnumerable<VehicleEngineTypeDto>>(entities);
        }

        public async Task<VehicleEngineTypeDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByID(id);
            if (entity == null)
                throw new KeyNotFoundException($"VehicleEngineType with ID {id} not found.");

            return _mapper.Map<VehicleEngineTypeDto>(entity);
        }
    }
}
