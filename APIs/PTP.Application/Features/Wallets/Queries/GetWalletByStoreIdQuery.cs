using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Wallets;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Wallets.Queries;

public class GetWalletByStoreIdQuery : IRequest<WalletViewModel>
{
    public Guid StoreId { get; set; } = default!;

    public class QueryValidation : AbstractValidator<GetWalletByStoreIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }

    public class QueryHandler : IRequestHandler<GetWalletByStoreIdQuery, WalletViewModel>
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
        public async Task<WalletViewModel> Handle(GetWalletByStoreIdQuery request, CancellationToken cancellationToken)
        {
            // if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            // var cacheResult = await _cacheService.GetByPrefixAsync<Wallet>(CacheKey.WALLET);
            // if (cacheResult!.Count>0)
            // {
            //     return _mapper.Map<WalletViewModel>(cacheResult.Where(x=>x.StoreId==request.StoreId));
            // }
            // var wallet = await _unitOfWork.WalletRepository.FirstOrDefaultAsync(x=>x.StoreId==request.StoreId,x=>x.Transactions,x=>x.WalletLogs);
            // if (wallet is null) throw new BadRequestException($"Store-{request.StoreId} is not exist any Wallet!");
            // await _cacheService.SetAsync<Wallet>(CacheKey.WALLET + wallet.Id, wallet);
            // return _mapper.Map<WalletViewModel>(wallet);
            return new WalletViewModel();
        }
    }
}