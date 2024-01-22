using MediatR;
using PTP.Application.ViewModels.Routes;

namespace PTP.Application.Features.Routes.Commands;
public class UpdateRouteCommand : IRequest<bool>
{
    public Guid Id { get; set; } = default!;
    public RouteUpdateModel Model { get; set; } = default!;
    public class CommandHandler : IRequestHandler<UpdateRouteCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
        {
            var route = await unitOfWork.RouteRepository.GetByIdAsync(request.Id) 
                        ?? throw new Exception($"no_data_found at {nameof(UpdateRouteCommand)}");
            unitOfWork.Mapper.Map(request.Model, route);
            unitOfWork.RouteRepository.Update(route);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}