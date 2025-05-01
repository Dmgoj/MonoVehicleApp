using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Entities;
using Project.Repository.Common;
using Project.Service.Common;
using Project.Service.Common.Exceptions;

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
            try
            {
                var entities = await _repository.Get();
                return _mapper.Map<IEnumerable<VehicleEngineTypeDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while retrieving engine types.", ex);
            }
        }

        public async Task<VehicleEngineTypeDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByID(id);
                if (entity == null)
                    throw new NotFoundException($"VehicleEngineType with ID {id} not found.");

                return _mapper.Map<VehicleEngineTypeDto>(entity);
            }
            catch (NotFoundException)
            {
                // Let the global handler convert this to 404
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An error occurred while retrieving engine type {id}.", ex);
            }
        }
    }
}
