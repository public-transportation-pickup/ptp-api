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
                var result = new StoreReportModel
                {
                    TotalOrderLast = GetTotalOrderLast(orders),
                    ProductMosts = await GetProductMosts(orders),
                    CustomerMosts = GetCustomerMosts(orders)
                };
                return result;
            }

            private List<DateValue> GetTotalOrderLast(List<Order> orders)
            {
                DateTime startOfLastWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).Date;
                DateTime endOfLastWeek = startOfLastWeek.AddDays(7).AddSeconds(-1);
                var totalOrderLast = orders
                                        .Where(o => o.CreationDate >= startOfLastWeek && o.CreationDate <= endOfLastWeek)
                                        .GroupBy(o => o.CreationDate.DayOfWeek)
                                        .Select(o => new DateValue { });
                return totalOrderLast.ToList();


                // List<Object> orderss = new List<Object>
                // {
                //     new  { OrderId = 1, OrderDate = new DateTime(2022, 12, 1) },
                //     new  { OrderId = 2, OrderDate = new DateTime(2022, 12, 2) },
                //     new  { OrderId = 3, OrderDate = new DateTime(2022, 11, 30) },
                //     new  { OrderId = 4, OrderDate = new DateTime(2022, 11, 29) },
                //     new  { OrderId = 5, OrderDate = new DateTime(2022, 11, 28) },
                //     new  { OrderId = 6, OrderDate = new DateTime(2022, 11, 27) }
                // };



                // var ordersFromLastWeek = orderss
                //     .Where(o => o.OrderDate >= startOfLastWeek && o.OrderDate <= endOfLastWeek)
                //     .GroupBy(o => o.OrderDate.DayOfWeek)
                //     .Select(g => new
                //     {
                //         DayOfWeek = g.Key.ToString(),
                //         OrderCount = g.Count()
                //     })
                //     .ToList();
                // throw new NotImplementedException();
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