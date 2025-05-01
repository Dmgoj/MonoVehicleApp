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
    public class VehicleRegistrationService : IVehicleRegistrationService
    {
        private readonly IRepository<VehicleRegistration> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleRegistrationService(
            IRepository<VehicleRegistration> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<VehicleRegistrationDto>> GetAllAsync(QueryParameters parameters)
        {
            try
            {
                Expression<Func<VehicleRegistration, bool>> filter = null;
                if (!string.IsNullOrWhiteSpace(parameters.Filter))
                {
                    var term = parameters.Filter.Trim().ToLower();
                    filter = r => r.RegistrationNumber.ToLower().Contains(term)
                             || r.VehicleOwner.FirstName.ToLower().Contains(term)
                             || r.VehicleOwner.LastName.ToLower().Contains(term);
                }

                var allItems = await _repository.Get(
                    filter: filter,
                    includeProperties: "VehicleModel,VehicleEngineType,VehicleOwner"
                );
                var totalCount = allItems.Count();

                Func<IQueryable<VehicleRegistration>, IOrderedQueryable<VehicleRegistration>> orderBy = null;
                if (!string.IsNullOrWhiteSpace(parameters.SortBy)
                    && Enum.TryParse<VehicleRegistrationSortField>(parameters.SortBy, true, out var sortField))
                {
                    bool desc = parameters.SortDescending;
                    switch (sortField)
                    {
                        case VehicleRegistrationSortField.RegistrationNumber:
                            orderBy = q => desc
                                ? q.OrderByDescending(r => r.RegistrationNumber)
                                : q.OrderBy(r => r.RegistrationNumber);
                            break;
                        case VehicleRegistrationSortField.VehicleModel:
                            orderBy = q => desc
                                ? q.OrderByDescending(r => r.VehicleModel.Name)
                                : q.OrderBy(r => r.VehicleModel.Name);
                            break;
                        case VehicleRegistrationSortField.VehicleEngineType:
                            orderBy = q => desc
                                ? q.OrderByDescending(r => r.VehicleEngineType.Type)
                                : q.OrderBy(r => r.VehicleEngineType.Type);
                            break;
                        case VehicleRegistrationSortField.VehicleOwner:
                            orderBy = q => desc
                                ? q.OrderByDescending(r => r.VehicleOwner.FirstName + " " + r.VehicleOwner.LastName)
                                : q.OrderBy(r => r.VehicleOwner.FirstName + " " + r.VehicleOwner.LastName);
                            break;
                    }
                }

                var page = await _repository.Get(
                    filter: filter,
                    orderBy: orderBy,
                    includeProperties: "VehicleModel,VehicleEngineType,VehicleOwner",
                    pagingParameters: parameters
                );

                var items = _mapper.Map<IEnumerable<VehicleRegistrationDto>>(page);

                return new PaginatedResult<VehicleRegistrationDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex) when (
                !(ex is NotFoundException) &&
                !(ex is ValidationException) &&
                !(ex is ConflictException))
            {
                throw new ServiceException("An error occurred while retrieving registrations.", ex);
            }
        }

        public async Task<VehicleRegistrationDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByID(id);
            if (entity == null)
                throw new NotFoundException($"VehicleRegistration with ID {id} not found.");

            return _mapper.Map<VehicleRegistrationDto>(entity);
        }

        public async Task<VehicleRegistrationDto> CreateAsync(VehicleRegistrationForCreateDto dto)
        {
            try
            {
                var entity = _mapper.Map<VehicleRegistration>(dto);
                await _repository.Insert(entity);
                await _unitOfWork.Save();

                return _mapper.Map<VehicleRegistrationDto>(entity);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException("An error occurred while creating the registration.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while creating the registration.", ex);
            }
        }

        public async Task UpdateAsync(int id, VehicleRegistrationForUpdateDto dto)
        {
            try
            {
                var entity = await _repository.GetByID(id);
                if (entity == null)
                    throw new NotFoundException($"VehicleRegistration with ID {id} not found.");

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
                throw new ConflictException("An error occurred while updating the registration.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error updating VehicleRegistration {id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByID(id);
                if (entity == null)
                    throw new NotFoundException($"VehicleRegistration with ID {id} not found.");

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
                throw new ConflictException("Cannot delete VehicleRegistration because it is referenced by other records.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error deleting VehicleRegistration {id}.", ex);
            }
        }
    }
}