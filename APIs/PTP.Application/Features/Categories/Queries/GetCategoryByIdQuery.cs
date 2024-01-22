using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Categories;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Categories.Queries;

public class GetCategoryByIdQuery:IRequest<CategoryViewModel>
{
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetCategoryByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }
    public class QueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryViewModel>
    {
         private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private ILogger<QueryHandler> _logger;

        public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, ILogger<QueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
        }
        public async Task<CategoryViewModel> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            var cacheResult = await _cacheService.GetAsync<Category>(CacheKey.CATE+request.Id);
            if (cacheResult is not null)
            {
                return _mapper.Map<CategoryViewModel>(cacheResult);
            }
            var cate = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id);
            if (cate is null) throw new BadRequestException($"Category with ID-{request.Id} is not exist!");
            await _cacheService.SetAsync<Category>(CacheKey.CATE + request.Id, cate);
            return _mapper.Map<CategoryViewModel>(cate);
        }
    }
}