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
    public class VehicleModelService : IVehicleModelService
    {
        private readonly IRepository<VehicleModel> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleModelService(
            IRepository<VehicleModel> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<VehicleModelDto>> GetAllAsync(QueryParameters parameters)
        {
            try
            {
                string term = string.IsNullOrWhiteSpace(parameters.Filter)
                    ? null
                    : parameters.Filter.Trim().ToLower();

                Expression<Func<VehicleModel, bool>> filter = m =>
                    (term == null || m.Name.ToLower().Contains(term))
                    && (!parameters.MakeId.HasValue || m.VehicleMakeId == parameters.MakeId.Value);

                
                var all = await _repository.Get(filter: filter);
                var totalCount = all.Count();

                Func<IQueryable<VehicleModel>, IOrderedQueryable<VehicleModel>> orderBy = null;
                if (!string.IsNullOrWhiteSpace(parameters.SortBy)
                    && Enum.TryParse<VehicleModelSortField>(parameters.SortBy, true, out var sortField))
                {
                    bool desc = parameters.SortDescending;
                    switch (sortField)
                    {
                        case VehicleModelSortField.Name:
                            orderBy = q => desc
                                ? q.OrderByDescending(x => x.Name)
                                : q.OrderBy(x => x.Name);
                            break;
                        case VehicleModelSortField.Abrv:
                            orderBy = q => desc
                                ? q.OrderByDescending(x => x.Abrv)
                                : q.OrderBy(x => x.Abrv);
                            break;
                    }
                }

                var page = await _repository.Get(
                    filter: filter,
                    orderBy: orderBy,
                    includeProperties: string.Empty,
                    pagingParameters: parameters
                );

                var items = _mapper.Map<IEnumerable<VehicleModelDto>>(page);

                return new PaginatedResult<VehicleModelDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while retrieving vehicle models.", ex);
            }
        }


        public async Task<VehicleModelDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByID(id);
                if (entity == null)
                    throw new NotFoundException($"VehicleModel with ID {id} not found.");

                return _mapper.Map<VehicleModelDto>(entity);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An error occurred while retrieving VehicleModel with ID {id}.", ex);
            }
        }

        public async Task<VehicleModelDto> CreateAsync(VehicleModelForCreateDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ValidationException("Vehicle model name is required.");

                var entity = _mapper.Map<VehicleModel>(dto);
                await _repository.Insert(entity);
                await _unitOfWork.Save();

                return _mapper.Map<VehicleModelDto>(entity);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException("An error occurred while creating vehicle model.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Unexpected error occurred while creating vehicle model.", ex);
            }
        }

        public async Task UpdateAsync(int id, VehicleModelForUpdateDto dto)
        {
            try
            {
                var entity = await _repository.GetByID(id);
                if (entity == null)
                    throw new NotFoundException($"VehicleModel with ID {id} not found.");

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ValidationException("Vehicle model name is required.");

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
                throw new ConflictException("An error occurred while updating vehicle model.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error updating VehicleModel {id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByID(id);
                if (entity == null)
                    throw new NotFoundException($"VehicleModel with ID {id} not found.");

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
                throw new ConflictException("Cannot delete VehicleModel because it is referenced by other records.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error deleting VehicleModel {id}.", ex);
            }
        }
    }
}
