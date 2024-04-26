using Amazon.Runtime.Internal;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.ViewModels.Stores;
using PTP.Domain.Enums;

namespace PTP.Application.Features.Stores.Queries;

public class GetStoreReportByDateQuery : IRequest<List<StoreReportByDate>>
{
    public Guid Id { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }

    public class QueryValidation : AbstractValidator<GetStoreReportByDateQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            RuleFor(x => x.ValidFrom).NotNull().NotEmpty().LessThanOrEqualTo(x => x.ValidTo).WithMessage("ValidFrom must less than or equal to ValidTo");
        }

        public class QueryHandler : IRequestHandler<GetStoreReportByDateQuery, List<StoreReportByDate>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<QueryHandler> _logger;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;

            }
            public async Task<List<StoreReportByDate>> Handle(GetStoreReportByDateQuery request, CancellationToken cancellationToken)
            {
                var orders = await _unitOfWork
                               .OrderRepository
                               .WhereAsync(x =>
                                   x.StoreId == request.Id &&
                                   x.CreationDate.Date >= request.ValidFrom &&
                                   x.CreationDate.Date <= request.ValidTo
                                   );
                var result = orders
                        .GroupBy(x => x.CreationDate.Date)
                        .Select(o => new StoreReportByDate
                        {
                            Date = o.Key!,
                            Id = o.FirstOrDefault()!.Id,
                            OrderCompleted = o.Count(x => x.Status == nameof(OrderStatusEnum.Completed)),
                            OrderCanceled = o.Count(x => x.Status == nameof(OrderStatusEnum.Canceled)),
                            Revenue = o.Sum(x => x.Total)
                        })
                        .OrderBy(s => s.Date)
                        .ToList();
                return result;
            }
        }
    }

}