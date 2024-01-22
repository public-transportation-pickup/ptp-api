using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Categories;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Categories.Queries;

public class GetAllCategoryQuery:IRequest<IEnumerable<CategoryViewModel>>
{
    public class QueryHandler : IRequestHandler<GetAllCategoryQuery, IEnumerable<CategoryViewModel>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService  _cacheService;
        private ILogger<QueryHandler> logger;

        public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, ILogger<QueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            this.logger = logger;
        }

        public async Task<IEnumerable<CategoryViewModel>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            var cacheResult = await _cacheService.GetByPrefixAsync<Category>(CacheKey.CATE);
            if (cacheResult!.Count > 0)
            {
                return _mapper.Map<IEnumerable<CategoryViewModel>>(cacheResult);
            }
            var cates = await _unitOfWork.CategoryRepository.GetAllAsync();
            if (cates.Count == 0) throw new NotFoundException("There are no category in DB!");
            await _cacheService.SetByPrefixAsync<Category>(CacheKey.CATE, cates);
            return _mapper.Map<IEnumerable<CategoryViewModel>>(cates);
        }
    }
}