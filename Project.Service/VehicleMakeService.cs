using System.Linq.Expressions;
using AutoMapper;
using Project.DAL.Entities;
using Project.Repository.Common;
using Project.Service.Common;
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
            // 1) Filter
            IEnumerable<VehicleMake> filtered = await _repository.Get(
                filter: string.IsNullOrWhiteSpace(parameters.Filter)
                    ? null
                    : (Expression<Func<VehicleMake, bool>>)(vm => vm.Name.ToLower().Contains(parameters.Filter.Trim().ToLower())));

            var totalCount = filtered.Count();

            // 2) Sort & page
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

        public async Task<VehicleMakeDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByID(id);
            if (entity == null)
                throw new KeyNotFoundException($"VehicleMake with ID {id} not found.");

            return _mapper.Map<VehicleMakeDto>(entity);
        }

        public async Task<VehicleMakeDto> CreateAsync(VehicleMakeForCreateDto dto)
        {
            var entity = _mapper.Map<VehicleMake>(dto);
            await _repository.Insert(entity);
            await _unitOfWork.Save();

            return _mapper.Map<VehicleMakeDto>(entity);
        }

        public async Task UpdateAsync(int id, VehicleMakeForUpdateDto dto)
        {
            var entity = await _repository.GetByID(id);
            if (entity == null)
                throw new KeyNotFoundException($"VehicleMake with ID {id} not found.");

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
