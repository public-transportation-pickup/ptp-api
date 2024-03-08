using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Stores;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Stores.Queries
{
    public class GetStoreByIdQuery : IRequest<StoreViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetStoreByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetStoreByIdQuery, StoreViewModel>
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

            public async Task<StoreViewModel> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken)
            {
                if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
                var cacheResult = await _cacheService.GetAsync<Store>(CacheKey.STORE + request.Id);
                if (cacheResult is not null)
                {
                    return _mapper.Map<StoreViewModel>(cacheResult);
                }
                var store = await _unitOfWork.StoreRepository.GetByIdAsync(request.Id, x => x.User);
                if (store is null) throw new BadRequestException($"Store with ID-{request.Id} is not exist!");
                await _cacheService.SetAsync<Store>(CacheKey.STORE + request.Id, store);
                return _mapper.Map<StoreViewModel>(store);
            }
        }
    }
}
