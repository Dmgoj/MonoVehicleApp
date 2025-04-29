using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Project.DAL.Entities;
using Project.Repository.Common;
using Project.Service.Common;
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
            // 1) Build filter expression if provided
            Expression<Func<VehicleOwner, bool>> filter = null;
            if (!string.IsNullOrWhiteSpace(parameters.Filter))
            {
                var term = parameters.Filter.Trim().ToLower();
                filter = o => o.FirstName.ToLower().Contains(term)
                            || o.LastName.ToLower().Contains(term);
            }

            // 2) Get total count
            var allOwners = await _repository.Get(filter: filter);
            var totalCount = allOwners.Count();

            // 3) Build ordering if requested
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

            // 4) Retrieve paged data
            var page = await _repository.Get(
                filter: filter,
                orderBy: orderBy,
                includeProperties: string.Empty,
                pagingParameters: parameters
            );

            // 5) Map to DTOs
            var items = _mapper.Map<IEnumerable<VehicleOwnerDto>>(page);

            return new PaginatedResult<VehicleOwnerDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        public async Task<VehicleOwnerDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByID(id);
            if (entity == null)
                throw new KeyNotFoundException($"VehicleOwner with ID {id} not found.");

            return _mapper.Map<VehicleOwnerDto>(entity);
        }

        public async Task<VehicleOwnerDto> CreateAsync(VehicleOwnerForCreateDto dto)
        {
            var entity = _mapper.Map<VehicleOwner>(dto);
            await _repository.Insert(entity);
            await _unitOfWork.Save();

            return _mapper.Map<VehicleOwnerDto>(entity);
        }

        public async Task UpdateAsync(int id, VehicleOwnerForUpdateDto dto)
        {
            var entity = await _repository.GetByID(id);
            if (entity == null)
                throw new KeyNotFoundException($"VehicleOwner with ID {id} not found.");

            _mapper.Map(dto, entity);
            await _repository.Update(entity);
            await _unitOfWork.Save();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
            await _unitOfWork.Save();
        }
    }
}
