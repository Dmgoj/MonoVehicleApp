using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Entities;
using Project.Repository.Common;
using Project.Service.Common;
using Project.Service.Common.Exceptions;
using Project.Service.Common.Parameters;

namespace Project.Service
{
    public class VehicleOwnerService : IVehicleOwnerService
    {
        private readonly IRepository<VehicleOwner> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleOwnerService(
            IRepository<VehicleOwner> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<VehicleOwnerDto>> GetAllAsync(QueryParameters parameters)
        {
            try
            {
                string term = string.IsNullOrWhiteSpace(parameters.Filter)
                    ? null
                    : parameters.Filter.Trim().ToLower();

                Expression<Func<VehicleOwner, bool>> filter = o =>
                    (term == null
                     || o.FirstName.ToLower().Contains(term)
                     || o.LastName.ToLower().Contains(term))
                    && (!parameters.MakeId.HasValue
                        || o.VehicleRegistrations
                            .Any(r => r.VehicleModel.VehicleMakeId == parameters.MakeId.Value))
                    && (!parameters.ModelId.HasValue
                        || o.VehicleRegistrations
                            .Any(r => r.VehicleModelId == parameters.ModelId.Value));

                var all = await _repository.Get(
                    filter: filter,
                    includeProperties: "VehicleRegistrations.VehicleModel,VehicleRegistrations.VehicleModel.VehicleMake"
                );
                var totalCount = all.Count();

                Func<IQueryable<VehicleOwner>, IOrderedQueryable<VehicleOwner>> orderBy = null;
                if (!string.IsNullOrWhiteSpace(parameters.SortBy)
                    && Enum.TryParse<VehicleOwnerSortField>(parameters.SortBy, true, out var sortField))
                {
                    bool desc = parameters.SortDescending;
                    switch (sortField)
                    {
                        case VehicleOwnerSortField.FirstName:
                            orderBy = q => desc
                                ? q.OrderByDescending(o => o.FirstName)
                                : q.OrderBy(o => o.FirstName);
                            break;
                        case VehicleOwnerSortField.LastName:
                            orderBy = q => desc
                                ? q.OrderByDescending(o => o.LastName)
                                : q.OrderBy(o => o.LastName);
                            break;
                    }
                }

                var page = await _repository.Get(
                    filter: filter,
                    orderBy: orderBy,
                    includeProperties: "VehicleRegistrations.VehicleModel,VehicleRegistrations.VehicleModel.VehicleMake",
                    pagingParameters: parameters
                );

                var items = _mapper.Map<IEnumerable<VehicleOwnerDto>>(page);

                return new PaginatedResult<VehicleOwnerDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while retrieving vehicle owners.", ex);
            }
        }






        public async Task<VehicleOwnerDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByID(id);
                if (entity == null)
                    throw new NotFoundException($"VehicleOwner with ID {id} not found.");

                return _mapper.Map<VehicleOwnerDto>(entity);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An error occurred while retrieving VehicleOwner with ID {id}.", ex);
            }
        }

        public async Task<VehicleOwnerDto> CreateAsync(VehicleOwnerForCreateDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
                    throw new ValidationException("First name and last name are required.");

                var entity = _mapper.Map<VehicleOwner>(dto);
                await _repository.Insert(entity);
                await _unitOfWork.Save();

                return _mapper.Map<VehicleOwnerDto>(entity);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException("An error occurred while creating vehicle owner.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Unexpected error occurred while creating vehicle owner.", ex);
            }
        }

        public async Task UpdateAsync(int id, VehicleOwnerForUpdateDto dto)
        {
            try
            {
                var entity = await _repository.GetByID(id);
                if (entity == null)
                    throw new NotFoundException($"VehicleOwner with ID {id} not found.");

                if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
                    throw new ValidationException("First name and last name are required.");

                _mapper.Map(dto, entity);
                await _repository.Update(entity);
                await _unitOfWork.Save();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConflictException("The record was modified by someone else. Please reload and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException("An error occurred while updating vehicle owner.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error updating VehicleOwner {id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByID(id);
                if (entity == null)
                    throw new NotFoundException($"VehicleOwner with ID {id} not found.");

                await _repository.Delete(id);
                await _unitOfWork.Save();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConflictException("The record was modified by someone else. Please reload and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException("Cannot delete VehicleOwner because it is referenced by other records.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error deleting VehicleOwner {id}.", ex);
            }
        }
    }
}
