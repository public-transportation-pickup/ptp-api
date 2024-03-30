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

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _cacheService = cacheService;
                _logger = logger;
            }


            public async Task<StoreReportModel> Handle(GetStoreReportById request, CancellationToken cancellationToken)
            {
                var orders = await _unitOfWork
                                .OrderRepository
                                .WhereAsync(x =>
                                    x.StoreId == request.Id &&
                                    x.Status == nameof(OrderStatusEnum.Completed)
                                    , x => x.User);
                var totalOrders = orders.Count > 0 ? orders.Count : 0;
                var totalSales = orders.Count > 0 ? orders.Sum(o => o.Total) : 0;
                var averageSaleValue = totalOrders != 0 ? (totalSales / totalOrders) : 0;
                var visitors = 0;
                var result = new StoreReportModel
                {
                    TotalSales = totalSales,
                    TotalOrders = totalOrders,
                    AverageSaleValue = averageSaleValue,
                    Visitors = visitors,
                    SaleValuesNew = GetSaleValuesNew(orders),
                    SaleValuesLast = GetSaleValuesLast(orders),
                    TotalOrderNew = GetTotalOrderNow(orders),
                    TotalOrderLast = GetTotalOrderLast(orders),
                    ProductMosts = await GetProductMosts(orders),
                    CustomerMosts = GetCustomerMosts(orders)
                };
                return result;
            }



            private List<decimal> GetSaleValuesNew(List<Order> orders)
            {
                DateTime startOfCurrent = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1).Date;
                DateTime endOfCurrent = startOfCurrent.AddDays(7).AddSeconds(-1);
                orders = orders.Where(o => o.CreationDate >= startOfCurrent && o.CreationDate <= endOfCurrent).ToList();
                var ordersFromLastWeek = Enumerable.Range(0, 7)
                                  .Select(i => orders
                                                .Where(o => o.CreationDate.DayOfWeek == ((DateTime)startOfCurrent.AddDays(i)).DayOfWeek)
                                                .Sum(x => x.Total))

                                  // .Select(i => new DateValue
                                  // {
                                  //     DayOfWeek = ((DateTime)startOfLastWeek.AddDays(i)).DayOfWeek.ToString(),
                                  //     Value = orders.Count(o => o.CreationDate.DayOfWeek == ((DateTime)startOfLastWeek.AddDays(i)).DayOfWeek)
                                  // })
                                  .ToList();
                return ordersFromLastWeek;
            }
            private List<decimal> GetSaleValuesLast(List<Order> orders)
            {
                DateTime startOfLastWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 6).Date;
                DateTime endOfLastWeek = startOfLastWeek.AddDays(7).AddSeconds(-1);
                orders = orders.Where(o => o.CreationDate >= startOfLastWeek && o.CreationDate <= endOfLastWeek).ToList();
                var ordersFromLastWeek = Enumerable.Range(0, 7)
                                  .Select(i => orders
                                                .Where(o => o.CreationDate.DayOfWeek == ((DateTime)startOfLastWeek.AddDays(i)).DayOfWeek)
                                                .Sum(x => x.Total))

                                  // .Select(i => new DateValue
                                  // {
                                  //     DayOfWeek = ((DateTime)startOfLastWeek.AddDays(i)).DayOfWeek.ToString(),
                                  //     Value = orders.Count(o => o.CreationDate.DayOfWeek == ((DateTime)startOfLastWeek.AddDays(i)).DayOfWeek)
                                  // })
                                  .ToList();
                return ordersFromLastWeek;
            }

            private List<int> GetTotalOrderNow(List<Order> orders)
            {
                DateTime startOfCurrent = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1).Date;
                DateTime endOfCurrent = startOfCurrent.AddDays(7).AddSeconds(-1);
                orders = orders.Where(o => o.CreationDate >= startOfCurrent && o.CreationDate <= endOfCurrent).ToList();
                var ordersFromCurrentWeek = Enumerable.Range(0, 7)
                    .Select(i => orders.Count(o => o.CreationDate.DayOfWeek == ((DateTime)startOfCurrent.AddDays(i)).DayOfWeek))
                    // .Select(i => new DateValue
                    // {
                    //     DayOfWeek = ((DateTime)startOfCurrent.AddDays(i)).DayOfWeek.ToString(),
                    //     Value = orders.Count(o => o.CreationDate.DayOfWeek == ((DateTime)startOfCurrent.AddDays(i)).DayOfWeek)
                    // })
                    .ToList();


                return ordersFromCurrentWeek;
            }
            private List<int> GetTotalOrderLast(List<Order> orders)
            {
                DateTime startOfLastWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 6).Date;
                DateTime endOfLastWeek = startOfLastWeek.AddDays(7).AddSeconds(-1);
                orders = orders.Where(o => o.CreationDate >= startOfLastWeek && o.CreationDate <= endOfLastWeek).ToList();
                var ordersFromLastWeek = Enumerable.Range(0, 7)
                    .Select(i => orders.Count(o => o.CreationDate.DayOfWeek == ((DateTime)startOfLastWeek.AddDays(i)).DayOfWeek))
                    // .Select(i => new DateValue
                    // {
                    //     DayOfWeek = ((DateTime)startOfLastWeek.AddDays(i)).DayOfWeek.ToString(),
                    //     Value = orders.Count(o => o.CreationDate.DayOfWeek == ((DateTime)startOfLastWeek.AddDays(i)).DayOfWeek)
                    // })
                    .ToList();


                return ordersFromLastWeek;
            }
            private async Task<List<ProductMost>> GetProductMosts(List<Order> orders)
            {
                var orderIds = orders.Select(x => x.Id);
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
                                TotalMoney = o.Sum(x => x.Total),
                                TotalOrder = o.Count(x => x.CreatedBy == o.Key),
                                PhoneNumber = o.FirstOrDefault()!.User.PhoneNumber,
                                FullName = o.FirstOrDefault()!.User.FullName
                            })
                            .OrderByDescending(s => s.TotalMoney)
                            .Take(5)
                            .ToList();

                return topCus;
            }
        }
    }
}