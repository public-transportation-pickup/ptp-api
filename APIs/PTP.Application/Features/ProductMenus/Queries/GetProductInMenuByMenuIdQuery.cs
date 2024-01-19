using AutoMapper;
using FluentValidation;
using MediatR;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.ProductMenus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.ProductMenus.Queries;

public class GetProductInMenuByMenuIdQuery:IRequest<IEnumerable<ProductMenuViewModel>>
{
    public Guid MenuId{get;set;}
    public class CommmandValidation : AbstractValidator<GetProductInMenuByMenuIdQuery>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.MenuId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
        
    }

    public class CommandHandler : IRequestHandler<GetProductInMenuByMenuIdQuery, IEnumerable<ProductMenuViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        public CommandHandler(IUnitOfWork unitOfWork,ICacheService cacheService,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cacheService=cacheService;
            _mapper=mapper;
        }

        public async Task<IEnumerable<ProductMenuViewModel>> Handle(GetProductInMenuByMenuIdQuery request, CancellationToken cancellationToken)
        {
            if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            var cacheResult = await _cacheService.GetByPrefixAsync<ProductInMenu>(CacheKey.PRODUCTMENU);
            if (cacheResult!.Count > 0)
            {
                return _mapper.Map<IEnumerable<ProductMenuViewModel>>(cacheResult);
            }
            var productMenus= await _unitOfWork.ProductInMenuRepository
                                .WhereAsync(x=>x.MenuId==request.MenuId,
                                            x=>x.Menu,
                                            x=>x.Product,
                                            x=>x.Product.Category);

            if(productMenus.Count==0) throw new NotFoundException($"There are no product for Menu-{request.MenuId}!");
            await _cacheService.SetByPrefixAsync<ProductInMenu>(CacheKey.PRODUCTMENU, productMenus);
            return _mapper.Map<IEnumerable<ProductMenuViewModel>>(productMenus);
        }
    }
}