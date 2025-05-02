using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Entities;
using Project.Repository.Common;
using Project.Service.Common;
using Project.Service.Common.Exceptions;
using Project.Service.Common.Parameters;

namespace Project.Service
{
    public class VehicleMakeService : IVehicleMakeService
    {
        private readonly IRepository<VehicleMake> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VehicleMakeService(IRepository<VehicleMake> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<VehicleMakeDto>> GetAllAsync(QueryParameters parameters)
        {
            try
            {
                IEnumerable<VehicleMake> filtered = await _repository.Get(
                    filter: string.IsNullOrWhiteSpace(parameters.Filter)
                        ? null
                        : (Expression<Func<VehicleMake, bool>>)(vm => vm.Name.ToLower().Contains(parameters.Filter.Trim().ToLower())),
                    orderBy: null,
                    includeProperties: string.Empty,
                    pagingParameters: null
                );

                var totalCount = filtered.Count();

                Func<IQueryable<VehicleMake>, IOrderedQueryable<VehicleMake>> orderBy = null;
                if (!string.IsNullOrWhiteSpace(parameters.SortBy) && Enum.TryParse<VehicleMakeSortField>(parameters.SortBy,
                                             ignoreCase: true,
                                             out var sortField))
                {
                    bool descending = parameters.SortDescending;
                    switch (sortField)
                    {
                        case VehicleMakeSortField.Name:
                            orderBy = q => descending
                              ? q.OrderByDescending(vm => vm.Name)
                              : q.OrderBy(vm => vm.Name);
                            break;

                        case VehicleMakeSortField.Abrv:
                            orderBy = q => descending
                              ? q.OrderByDescending(vm => vm.Abrv)
                              : q.OrderBy(vm => vm.Abrv);
                            break;
                    }
                }

                var pagedEntities = await _repository.Get(
                    filter: string.IsNullOrWhiteSpace(parameters.Filter)
                        ? null
                        : (Expression<Func<VehicleMake, bool>>)(vm => vm.Name.ToLower().Contains(parameters.Filter.Trim().ToLower())),
                    orderBy: orderBy,
                    includeProperties: string.Empty,
                    pagingParameters: parameters
                );

                var items = _mapper.Map<IEnumerable<VehicleMakeDto>>(pagedEntities);

                return new PaginatedResult<VehicleMakeDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while retrieving vehicle makes.", ex);
            }
        }

        public async Task<VehicleMakeDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByID(id);
            if (entity == null)
                throw new NotFoundException($"VehicleMake with ID {id} not found.");

            return _mapper.Map<VehicleMakeDto>(entity);
        }

        public async Task<VehicleMakeDto> CreateAsync(VehicleMakeForCreateDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ValidationException("Vehicle make name is required.");

                var entity = _mapper.Map<VehicleMake>(dto);
                await _repository.Insert(entity);
                await _unitOfWork.Save();

                return _mapper.Map<VehicleMakeDto>(entity);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException("An error occurred while creating vehicle make.", ex);
            }
        }

        public async Task UpdateAsync(int id, VehicleMakeForUpdateDto dto)
        {
            try
            {
                var entity = await _repository.GetByID(id);
                if (entity == null)
                    throw new NotFoundException($"VehicleMake with ID {id} not found.");

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ValidationException("Vehicle make name is required.");

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
                throw new ConflictException(
                    "The record was modified by someone else. Please reload and try again.",
                    ex);
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException("An error occurred while updating vehicle make.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error updating VehicleMake {id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByID(id);
                if (entity == null)
                    throw new NotFoundException($"VehicleMake with ID {id} not found.");

                await _repository.Delete(id);
                await _unitOfWork.Save();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConflictException(
                    "The record was modified by someone else. Please reload and try again.",
                    ex);
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException("Cannot delete VehicleMake because it is referenced by other records.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error deleting VehicleMake {id}.", ex);
            }
        }

    }
}
