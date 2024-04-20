using System.Runtime.CompilerServices;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.Features.Orders.Queries;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Products;
using PTP.Domain.Enums;

namespace PTP.Application.Features.Products.Queries;
public class GetTopProductQuery : IRequest<TopProductViewModel?>
{
    public class QueryHandler : IRequestHandler<GetTopProductQuery, TopProductViewModel?>
    {
        private readonly IClaimsService claimsService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<GetTopProductQuery> logger;
        private readonly IMediator mediator;
        const string toolService = nameof(GetTopProductQuery);
        public QueryHandler(IClaimsService claimsService,
            IUnitOfWork unitOfWork,
            ILogger<GetTopProductQuery> logger,
            IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
            this.claimsService = claimsService;
        }
        public async Task<TopProductViewModel?> Handle(GetTopProductQuery request, CancellationToken cancellationToken)
        {

            Guid userId = claimsService.GetCurrentUser;
            logger.LogInformation($"Source: {toolService}_UserId : {userId}");
            var topProducts = await GetTopProductModelAsync(userId);
            var orders = await mediator.Send(new GetOrdersByUserIdQuery { UserId = userId, PageSize = 1000, PageNumber = -1, Filter = new() }, cancellationToken);

            logger.LogInformation($"Source: {toolService}_Products : {topProducts?.Count}");
            logger.LogInformation($"Source: {toolService}_Orders : {orders?.Count}");
            var result = new TopProductViewModel
            {
                Products = topProducts ?? new(),
                Orders = orders?.Where(x => x.Status == nameof(OrderStatusEnum.Completed))
                    .OrderByDescending(x => x.OrderDetails?.Count())
                    .Take(5)
                    .ToList() ?? new()
            };
            return result;
        }
        private async Task<List<TopProductModel>?> GetTopProductModelAsync(Guid userId)
        {
            using var connection = unitOfWork.DirectionConnection.GetDbConnection();
            string sql = SqlQueriesStorage.GET_TOP_PRODUCT_BY_USER;
            DynamicParameters parameters = new();
            parameters.Add("@UserId", userId);
            var executeResult = await connection.QueryAsync<TopProductModel>(sql: sql,
                param: parameters,
                transaction: null,
                commandTimeout: 30,
                commandType: System.Data.CommandType.Text);
            return executeResult?.ToList();
        }
    }
}