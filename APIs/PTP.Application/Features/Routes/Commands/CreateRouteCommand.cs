using System.Reflection.Metadata.Ecma335;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using PTP.Application.ViewModels.Routes;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Routes.Commands;
public class CreateRouteCommand : IRequest<RouteViewModel?>
{

    public RouteCreateModel Model { get; set; } = default!;
    public class CommandValidation : AbstractValidator<CreateRouteCommand>
    {
        public CommandValidation() 
        {
            RuleFor(x => x.Model.RouteNo).NotNull().NotEmpty().WithMessage($"RouteNoIsNotValid");
            RuleFor(x => x.Model.NumOfSeats)
            .NotNull()
            .NotEmpty()
            .Matches(@"^\d+(?:-\d+)?$")
            .WithMessage(@"Num of seats is not valid. Must be in form 'XX-XX', 'XX', with X is number");
            RuleFor(x => x.Model.TimeOfTrip)
            .NotNull()
            .NotEmpty()
            .Matches(@"^\d+(?:-\d+)?$")
            .WithMessage(@"Time of trip not valid. Must be in form 'XX-XX', 'XX', with X is number");

        }
    }
    public class CommandHandler : IRequestHandler<CreateRouteCommand, RouteViewModel?>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<RouteViewModel?> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
        {
            var route = _unitOfWork.Mapper.Map<Route>(request.Model);
            await _unitOfWork.RouteRepository.AddAsync(route);
            return await _unitOfWork.SaveChangesAsync()
                ? _unitOfWork.Mapper.Map<RouteViewModel>(await _unitOfWork.RouteRepository.GetByIdAsync(route.Id))
                : throw new Exception($"Error: {nameof(CreateRouteCommand)}Save Change Failed");
        }
    }
}