using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.ProductMenus;
using PTP.Domain.Globals;

namespace PTP.Application.Features.ProductMenus.Commands;
public class UpdateProductMenuCommand : IRequest<bool>
{
    public ProductMenuUpdateModel UpdateModel { get; set; } = default!;
    public class CommmandValidation : AbstractValidator<UpdateProductMenuCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.UpdateModel.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            RuleFor(x => x.UpdateModel.SalePrice).GreaterThan(0).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            RuleFor(x => x.UpdateModel.Status).NotNull().NotEmpty().WithMessage("Status must not null or empty");
            RuleFor(x => x.UpdateModel.PreparationTime).GreaterThan(0).NotNull().NotEmpty().WithMessage("PreparationTime must not null or empty");
            RuleFor(x => x.UpdateModel.NumProcessParallel).GreaterThan(0).NotNull().NotEmpty().WithMessage("NumProcessParallel must not null or empty");
        }
    }

    public class CommandHandler : IRequestHandler<UpdateProductMenuCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private ILogger<CommandHandler> _logger;
        public CommandHandler(IUnitOfWork unitOfWork,
                ICacheService cacheService,
                ILogger<CommandHandler> logger,
                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateProductMenuCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update ProductMenu:\n");
            //Remove From Cache       

            var productMenu = await _unitOfWork.ProductInMenuRepository.GetByIdAsync(request.UpdateModel.Id);
            if (productMenu is null) throw new NotFoundException($"ProductMenu with Id-{request.UpdateModel.Id} is not exist!");

            productMenu = _mapper.Map(request.UpdateModel, productMenu);

            _unitOfWork.ProductInMenuRepository.Update(productMenu);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result)
            {
                if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
                await _cacheService.RemoveByPrefixAsync(CacheKey.PRODUCTMENU);
            }
            return result;
        }
    }
}