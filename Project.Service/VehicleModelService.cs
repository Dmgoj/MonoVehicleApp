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
            Expression<Func<VehicleModel, bool>> filter = null;
            if (!string.IsNullOrWhiteSpace(parameters.Filter))
            {
                var term = parameters.Filter.Trim().ToLower();
                filter = m => m.Name.ToLower().Contains(term);
            }

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

        public async Task<VehicleModelDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByID(id);
            if (entity == null)
                throw new KeyNotFoundException($"VehicleModel with ID {id} not found.");

            return _mapper.Map<VehicleModelDto>(entity);
        }

        public async Task<VehicleModelDto> CreateAsync(VehicleModelForCreateDto dto)
        {
            var entity = _mapper.Map<VehicleModel>(dto);
            await _repository.Insert(entity);
            await _unitOfWork.Save();

            return _mapper.Map<VehicleModelDto>(entity);
        }

        public async Task UpdateAsync(int id, VehicleModelForUpdateDto dto)
        {
            var entity = await _repository.GetByID(id);
            if (entity == null)
                throw new KeyNotFoundException($"VehicleModel with ID {id} not found.");

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
