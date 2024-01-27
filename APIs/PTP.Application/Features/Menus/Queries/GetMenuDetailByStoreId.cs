using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;
using System;
using System.Collections.Generic;
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
            private ILogger<QueryHandler> _logger;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _cacheService = cacheService;
                _logger = logger;
            }
            public async Task<MenuViewModel> Handle(GetMenuDetailByStoreId request, CancellationToken cancellationToken)
            {
                if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
                var cacheResult = await _cacheService.GetByPrefixAsync<Menu>(CacheKey.MENU);
                if (cacheResult!.Count > 0)
                {
                    //var result = cacheResult.Select(x => x.StoreId == request.StoreId);
                    var result = cacheResult.Where(x => x.StoreId == request.StoreId);
                    return _mapper.Map<MenuViewModel>(result);
                }
                var menus = await _unitOfWork.MenuRepository.WhereAsync(x => x.StoreId == request.StoreId, x => x.Store);
                if (menus is null) throw new BadRequestException($"Store with ID-{request.StoreId} is not exist any menus!");
                await _cacheService.SetByPrefixAsync<Menu>(CacheKey.MENU, menus);
                return _mapper.Map<MenuViewModel>(menus);
            }
        }
    }
}
