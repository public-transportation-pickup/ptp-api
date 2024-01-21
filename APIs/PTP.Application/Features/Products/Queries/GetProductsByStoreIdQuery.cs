using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Products;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Products.Queries;

public class GetProductsByStoreIdQuery:IRequest<IEnumerable<ProductViewModel>>
{
    public Guid StoreId { get; set; } = default!;
    public Guid CategoryId{get;set;}=default!;

   public class QueryValidation : AbstractValidator<GetProductsByStoreIdQuery>
   {
       public QueryValidation()
       {
           RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("Id must not null or empty");
       }
   }
   public class QueryHandler : IRequestHandler<GetProductsByStoreIdQuery, IEnumerable<ProductViewModel>>
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
       public async Task<IEnumerable<ProductViewModel>> Handle(GetProductsByStoreIdQuery request, CancellationToken cancellationToken)
       {
           if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
           var cacheResult = await _cacheService.GetByPrefixAsync<Product>(CacheKey.PRODUCT);
           if (cacheResult!.Count>0)
           {
               return request.CategoryId == Guid.Empty 
                    ? _mapper.Map<IEnumerable<ProductViewModel>>(cacheResult.Where(x=>x.StoreId==request.StoreId))
                    : _mapper.Map<IEnumerable<ProductViewModel>>(cacheResult.Where(x => x.StoreId == request.StoreId&&x.CategoryId==request.CategoryId));
           }
           var product = await _unitOfWork.ProductRepository.WhereAsync(x=>x.StoreId==request.StoreId,x=>x.Store,x=>x.Category);
           if (product is null) throw new BadRequestException($"Store with ID-{request.StoreId} is not exist any products!");
           await _cacheService.SetByPrefixAsync<Product>(CacheKey.PRODUCT, product);
           var result= _mapper.Map<IEnumerable<ProductViewModel>>(product);
           return request.CategoryId==Guid.Empty? result: result.Where(x=>x.CategoryId==request.CategoryId);
       }
   }
}