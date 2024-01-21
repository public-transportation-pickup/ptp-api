using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Menus;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Menus.Queries;

public class GetMenuByIdQuery:IRequest<MenuViewModel>
{
    public Guid Id { get; set; } = default!;

    public class QueryValidation : AbstractValidator<GetMenuByIdQuery>
    {
        public QueryValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
        }
    }
    public class QueryHandler : IRequestHandler<GetMenuByIdQuery, MenuViewModel>
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
        public async Task<MenuViewModel> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
        {
            if (_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            var cacheResult = await _cacheService.GetAsync<Menu>(CacheKey.MENU+request.Id);
            if (cacheResult is not null)
            {
                return _mapper.Map<MenuViewModel>(cacheResult);
            }
            var menu = await _unitOfWork.MenuRepository.GetByIdAsync(request.Id,x=>x.Store);
            if (menu is null) throw new BadRequestException($"Menu with ID-{request.Id} is not exist!");
            await _cacheService.SetAsync<Menu>(CacheKey.MENU + request.Id, menu);
            return _mapper.Map<MenuViewModel>(menu);
        }
    }
}