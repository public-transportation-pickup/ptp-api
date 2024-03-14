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
    public class GetStoreByUserIdQuery : IRequest<StoreViewModel>
    {
        public Guid UserId { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetStoreByUserIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("UserId must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetStoreByUserIdQuery, StoreViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ICacheService _cacheService;
            private readonly IClaimsService _claimService;
            private ILogger<QueryHandler> _logger;

            public QueryHandler(IUnitOfWork unitOfWork,
                                IMapper mapper,
                                ICacheService cacheService,
                                ILogger<QueryHandler> logger,
                                IClaimsService claimsService)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _cacheService = cacheService;
                _logger = logger;
                _claimService = claimsService;
            }

            public async Task<StoreViewModel> Handle(GetStoreByUserIdQuery request, CancellationToken cancellationToken)
            {
                if (!_claimService.GetCurrentUser.Equals(request.UserId)) throw new BadRequestException("UserId is not valid!");
                if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
                var cacheResult = await _cacheService.GetAsync<Store>(CacheKey.STORE + request.UserId);
                if (cacheResult is not null)
                {
                    return _mapper.Map<StoreViewModel>(cacheResult);
                }
                var store = await _unitOfWork.StoreRepository.FirstOrDefaultAsync(x => x.UserId == request.UserId, x => x.User);
                if (store is null) throw new BadRequestException($"Store with UserID-{request.UserId} is not exist!");
                await _cacheService.SetAsync<Store>(CacheKey.STORE + request.UserId, store);
                return _mapper.Map<StoreViewModel>(store);
            }
        }
    }
}
