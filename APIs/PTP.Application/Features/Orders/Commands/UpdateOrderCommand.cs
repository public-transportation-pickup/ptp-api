using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Features.Categories.Commands;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Orders;
using PTP.Domain.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.Features.Orders.Commands
{
    public class UpdateOrderCommand:IRequest<bool>
    {
        public OrderUpdateModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateOrderCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.UpdateModel.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
                RuleFor(x => x.UpdateModel.Status).NotNull().NotEmpty().WithMessage("Status must not null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<UpdateOrderCommand, bool>
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

            public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update Order:\n");
                //Remove From Cache       

                var order = await _unitOfWork.OrderRepository.GetByIdAsync(request.UpdateModel.Id);
                if (order is null) throw new NotFoundException($"Category with Id-{request.UpdateModel.Id} is not exist!");

                order = _mapper.Map(request.UpdateModel, order);

                _unitOfWork.OrderRepository.Update(order);
                var result = await _unitOfWork.SaveChangesAsync();
                return result;
            }
        }
    }

}
