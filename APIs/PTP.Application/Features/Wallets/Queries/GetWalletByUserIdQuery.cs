using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Transactions;
using PTP.Application.ViewModels.Wallets;
using PTP.Domain.Entities;
using PTP.Domain.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.Features.Wallets.Queries
{
    public class GetWalletByUserIdQuery : IRequest<WalletViewModel>
    {
        public Guid UserId { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetWalletByUserIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetWalletByUserIdQuery, WalletViewModel>
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
            public async Task<WalletViewModel> Handle(GetWalletByUserIdQuery request, CancellationToken cancellationToken)
            {
                var wallet = await _unitOfWork.WalletRepository.FirstOrDefaultAsync(x => x.UserId == request.UserId, x => x.WalletLogs);
                if (wallet is null) throw new BadRequestException($"User-{request.UserId} is not exist any Wallet!");

                var result = _mapper.Map<WalletViewModel>(wallet);
                result.Transactions = await GetTransactions(wallet.Id);
                return result;
            }

            private async Task<List<TransactionViewModel>> GetTransactions(Guid walletId)
            {
                var transaction = await _unitOfWork.TransactionRepository.WhereAsync(x => x.WalletId == walletId, x => x.Payment!, x => x.Payment!.Order);
                return _mapper.Map<List<TransactionViewModel>>(transaction);
            }
        }
    }
}
