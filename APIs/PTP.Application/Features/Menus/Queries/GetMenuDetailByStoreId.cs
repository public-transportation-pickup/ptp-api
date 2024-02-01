using AutoMapper;
using Azure.Core;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Features.ProductMenus.Queries;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.IntergrationServices.Models;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.Features.Menus.Queries
{
    public class GetMenuDetailByStoreId:IRequest<MenuViewModel>
    {
        public Guid StoreId { get; set; } = default!;
        public string ArrivalTime {  get; set; }=default!;

        public class QueryValidation : AbstractValidator<GetMenuDetailByStoreId>
        {
            public QueryValidation()
            {
                RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetMenuDetailByStoreId, MenuViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ICacheService _cacheService;
            private readonly IMediator _mediator;
            private ILogger<QueryHandler> _logger;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, IMediator mediator, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _cacheService = cacheService;
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<MenuViewModel> Handle(GetMenuDetailByStoreId request, CancellationToken cancellationToken)
            {
                //if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
                //var cacheResult = await _cacheService.GetByPrefixAsync<Menu>(CacheKey.MENU);
                //if (cacheResult!.Count > 0)
                //{
                //    var result = cacheResult.FirstOrDefault(x => 
                //                                 x.StoreId == request.StoreId &&
                //                                 ConvertToTime(x.StartTime)<ConvertToTime(request.ArrivalTime) &&
                //                                 ConvertToTime(x.EndTime) > ConvertToTime(request.ArrivalTime));
                    
                //}
                
                var menus = await _unitOfWork.MenuRepository.WhereAsync(x =>
                                                 x.StoreId == request.StoreId, x => x.Store);


                if (menus.Count==0) throw new BadRequestException($"Store with ID-{request.StoreId} is not exist any menus!");
                //await _cacheService.SetAsync<Menu>(CacheKey.MENU, menu);
                //var result= _mapper.Map<MenuViewModel>(menu);
                //result.ProductInMenus= await _mediator.Send(new GetProductInMenuByMenuIdQuery { MenuId=menu.Id });
                //return result; 
                var result = _mapper.Map<MenuViewModel>(GetMenu(menus, request.ArrivalTime));
                result.ProductInMenus = await _mediator.Send(new GetProductInMenuByMenuIdQuery { MenuId = result.Id });
                return result;
            }

            private Menu GetMenu(IEnumerable<Menu> menus,string arrivalTime) 
            {
                foreach (var item in menus)
                {
                    TimeSpan.TryParseExact(item.StartTime, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan startTime);
                    TimeSpan.TryParseExact(item.EndTime, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan endTime);
                    TimeSpan.TryParseExact(arrivalTime, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan aTime);
                    if (startTime < aTime && endTime > aTime) return item;
                }

                throw new BadRequestException($"No menu ready for arrival time {arrivalTime}");
            }

        }
    }
}
