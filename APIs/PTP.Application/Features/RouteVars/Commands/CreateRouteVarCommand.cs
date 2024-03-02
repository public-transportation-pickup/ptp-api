using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.RouteVars;
using PTP.Domain.Entities;

namespace PTP.Application.Features.RouteVars.Commands;
public class CreateRouteVarCommand : IRequest<RouteVarViewModel?>
{
    public RouteVarCreateModel Model { get; set; } = null!;
    public class CommandValidation : AbstractValidator<CreateRouteVarCommand>
    {
        public CommandValidation()
        {
            RuleFor(req => req.Model.RouteId).NotNull().NotEmpty();
            RuleFor(req => req.Model.RouteVarName).NotNull().NotEmpty();
        }
    }
    public class CommandHandler : IRequestHandler<CreateRouteVarCommand, RouteVarViewModel?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<RouteVarViewModel?> Handle(CreateRouteVarCommand request, CancellationToken cancellationToken)
        {

            if (_unitOfWork.RouteRepository.GetByIdAsync(request.Model.RouteId) is null)
            {
                throw new Exception($"{nameof(CreateRouteVarCommand)} : Route not exist! Can not create, RouteId: {request.Model.RouteId}");
            }
            var routeVar = _unitOfWork.Mapper.Map<RouteVar>(request.Model);
            await _unitOfWork.RouteVarRepository.AddAsync(routeVar);
            await _unitOfWork.SaveChangesAsync();
            var routeStation = request.Model.RouteStations.Select(x =>
                {
                    return new RouteStation
                    {
                        Id = Guid.NewGuid(),
                        RouteVarId = routeVar.Id,
                        StationId = x.StationId,
                        Index = x.Index,
                        Description = x.Description ?? string.Empty,
                        IsDeleted = false,
                        RouteId = routeVar.RouteId
                    };
                }).ToList();
            if(routeStation? .Count > 0)
                await _unitOfWork.RouteStationRepository.AddRangeAsync(routeStation);
            return await _unitOfWork.SaveChangesAsync() ?
            _unitOfWork.Mapper.Map<RouteVarViewModel>(await _unitOfWork.RouteVarRepository.GetByIdAsync(routeVar.Id)) :
            throw new Exception($"{nameof(CreateRouteVarCommand)}Save Change Failed!");


        }
    }
}