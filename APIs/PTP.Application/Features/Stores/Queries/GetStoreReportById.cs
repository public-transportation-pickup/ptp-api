using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Stores;
using PTP.Domain.Entities;
using PTP.Domain.Enums;

namespace PTP.Application.Features.Stores.Queries;

public class GetStoreReportById : IRequest<StoreReportModel>
{
    public Guid Id { get; set; } = default!;

    public class QueryValidation : AbstractValidator<GetStoreByIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }

        public class QueryHandler : IRequestHandler<GetStoreReportById, StoreReportModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ICacheService _cacheService;
            private ILogger<QueryHandler> _logger;

            private readonly DateTime _startOfCurrent;
            private readonly DateTime _startOfLastWeek;

            private readonly DateTime _endOfCurrent;
            private readonly DateTime _endOfLastWeek;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _cacheService = cacheService;
                _logger = logger;
                DateTime today = DateTime.Today;
                if (today.DayOfWeek == DayOfWeek.Sunday)
                {
                    _startOfCurrent = today.AddDays(-6).Date;
                }
                else
                {
                    _startOfCurrent = today.AddDays(-(int)today.DayOfWeek + 1).Date;
                }
                _startOfLastWeek = _startOfCurrent.AddDays(-7).Date;
                _endOfCurrent = _startOfCurrent.AddDays(6).Date;
                _endOfLastWeek = _startOfLastWeek.AddDays(6).Date;
            }


            public async Task<StoreReportModel> Handle(GetStoreReportById request, CancellationToken cancellationToken)
            {
                var store = await _unitOfWork.StoreRepository.GetByIdAsync(request.Id, x => x.User.Wallet);
                var orders = await _unitOfWork
                                .OrderRepository
                                .WhereAsync(x =>
                                    x.StoreId == request.Id
                                    // && x.Status == nameof(OrderStatusEnum.Completed)
                                    , x => x.User);
                var result = new StoreReportModel
                {
                    StoreId = request.Id,
                    StoreName = store!.Name,
                    StoreAddress = store.AddressNo,
                    WalletAmount = store.User.Wallet.Amount,
                    SaleValuesNew = GetSaleValuesNew(orders),
                    SaleValuesLast = GetSaleValuesLast(orders),
                    TotalOrderNew = GetTotalOrderNow(orders),
                    TotalOrderLast = GetTotalOrderLast(orders),
                    ProductMosts = await GetProductMosts(orders),
                    CustomerMosts = GetCustomerMosts(orders)
                };

                result = GetOtherNewReport(orders, result);
                result = GetOtherLastReport(orders, result);
                return result;
            }

            private StoreReportModel GetOtherNewReport(List<Order> orders, StoreReportModel model)
            {
                orders = orders.Where(x => x.CreationDate >= _startOfCurrent).ToList();
                model.TotalOrdersNew = orders.Count > 0 ? orders.Where(x => x.Status == nameof(OrderStatusEnum.Completed)).ToList().Count : 0;
                model.TotalSalesNew = orders.Count > 0 ? orders.Sum(o => o.Total) - orders.Where(x => x.ReturnAmount != null).Sum(x => x.ReturnAmount!.Value) : 0;
                model.AverageSaleValueNew = model.TotalOrdersNew != 0 ? (model.TotalSalesNew / model.TotalOrdersNew) : 0;
                model.VisitorsNew = orders.GroupBy(x => x.CreatedBy).Count();
                return model;
            }
            private StoreReportModel GetOtherLastReport(List<Order> orders, StoreReportModel model)
            {
                orders = orders.Where(x => x.CreationDate >= _startOfLastWeek && x.CreationDate <= _endOfLastWeek).ToList();
                model.TotalOrdersLast = orders.Count > 0 ? orders.Where(x => x.Status == nameof(OrderStatusEnum.Completed)).ToList().Count : 0;
                model.TotalSalesLast = orders.Count > 0 ? orders.Sum(o => o.Total) - orders.Where(x => x.ReturnAmount != null).Sum(x => x.ReturnAmount!.Value) : 0;
                model.AverageSaleValueLast = model.TotalOrdersLast != 0 ? (model.TotalSalesLast / model.TotalOrdersLast) : 0;
                model.VisitorsLast = orders.GroupBy(x => x.CreatedBy).Count();
                return model;
            }


            private List<decimal> GetSaleValuesNew(List<Order> orders)
            {

                orders = orders.Where(o => o.CreationDate >= _startOfCurrent && o.CreationDate <= _endOfCurrent).ToList();
                var ordersFromLastWeek = Enumerable.Range(0, 7)
                                  .Select(i =>
                                                orders
                                                    .Where(o => o.CreationDate.DayOfWeek == ((DateTime)_startOfCurrent.AddDays(i)).DayOfWeek)
                                                    .Sum(x => x.Total)
                                                -
                                                orders
                                                    .Where(x => x.CreationDate.DayOfWeek == ((DateTime)_startOfCurrent.AddDays(i)).DayOfWeek && x.ReturnAmount != null)
                                                    .Sum(x => x.ReturnAmount!.Value)
                                         )
                                  .ToList();
                return ordersFromLastWeek;
            }
            private List<decimal> GetSaleValuesLast(List<Order> orders)
            {
                orders = orders.Where(o => o.CreationDate >= _startOfLastWeek && o.CreationDate <= _endOfLastWeek).ToList();
                var ordersFromLastWeek = Enumerable.Range(0, 7)
                                  .Select(i =>
                                                orders
                                                    .Where(o => o.CreationDate.DayOfWeek == ((DateTime)_startOfCurrent.AddDays(i)).DayOfWeek)
                                                    .Sum(x => x.Total)
                                                -
                                                orders
                                                    .Where(x => x.CreationDate.DayOfWeek == ((DateTime)_startOfCurrent.AddDays(i)).DayOfWeek && x.ReturnAmount != null)
                                                    .Sum(x => x.ReturnAmount!.Value)
                                         )
                                  .ToList();
                return ordersFromLastWeek;
            }

            private List<int> GetTotalOrderNow(List<Order> orders)
            {
                orders = orders.Where(o => o.CreationDate >= _startOfCurrent && o.CreationDate <= _endOfCurrent && o.Status == nameof(OrderStatusEnum.Completed)).ToList();
                var ordersFromCurrentWeek = Enumerable.Range(0, 7)
                    .Select(i => orders.Count(o => o.CreationDate.DayOfWeek == ((DateTime)_startOfCurrent.AddDays(i)).DayOfWeek))
                    .ToList();


                return ordersFromCurrentWeek;
            }
            private List<int> GetTotalOrderLast(List<Order> orders)
            {
                orders = orders.Where(o => o.CreationDate >= _startOfLastWeek && o.CreationDate <= _endOfLastWeek && o.Status == nameof(OrderStatusEnum.Completed)).ToList();
                var ordersFromLastWeek = Enumerable.Range(0, 7)
                    .Select(i => orders.Count(o => o.CreationDate.DayOfWeek == ((DateTime)_startOfLastWeek.AddDays(i)).DayOfWeek))
                    .ToList();


                return ordersFromLastWeek;
            }
            private async Task<List<ProductMost>> GetProductMosts(List<Order> orders)
            {
                var orderIds = orders.Where(x => x.Status == nameof(OrderStatusEnum.Completed)).Select(x => x.Id);
                var orderDetails = await _unitOfWork.OrderDetailRepository
                                                    .WhereAsync(x =>
                                                        orderIds.Contains(x.OrderId),
                                                        x => x.ProductInMenu,
                                                        x => x.ProductInMenu.Product);
                var topProducts = orderDetails.
                                 GroupBy(x => x.ProductInMenu.ProductId).
                                 Select(p => new ProductMost
                                 {
                                     Id = p.Key,
                                     ProductName = p.FirstOrDefault()!.ProductInMenu.Product.Name,
                                     ImageURL = p.FirstOrDefault()!.ProductInMenu.Product.ImageURL,
                                     Price = p.FirstOrDefault()!.ProductInMenu.Product.Price,
                                     TotalQuantity = p.Sum(x => x.Quantity)
                                 })
                                .OrderByDescending(s => s.TotalQuantity)
                                .Take(5)
                                .ToList();
                return topProducts;
            }
            private List<CustomerMost> GetCustomerMosts(List<Order> orders)
            {
                var topCus = orders
                            .GroupBy(x => x.CreatedBy)
                            .Select(o => new CustomerMost
                            {
                                Id = o.Key!.Value,
                                TotalMoney = o.Sum(x => x.Total) - o.Where(x => x.ReturnAmount != null).Sum(x => x.ReturnAmount!.Value),
                                TotalOrder = o.Where(x => x.Status == nameof(OrderStatusEnum.Completed)).Count(x => x.CreatedBy == o.Key),
                                PhoneNumber = o.Where(x => x.Status == nameof(OrderStatusEnum.Completed)).FirstOrDefault()!.User.PhoneNumber,
                                FullName = o.Where(x => x.Status == nameof(OrderStatusEnum.Completed)).FirstOrDefault()!.Name
                            })
                            .OrderByDescending(s => s.TotalMoney)
                            .Take(5)
                            .ToList();

                return topCus;
            }
        }
    }
}