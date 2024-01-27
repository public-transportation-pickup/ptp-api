using System.Runtime.CompilerServices;
using MediatR;

namespace PTP.Application.Features.Stations.Commands;
public class DeleteStationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public class CommandHandler : IRequestHandler<DeleteStationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteStationCommand request, CancellationToken cancellationToken)
        {
            var station = await _unitOfWork.StationRepository.GetByIdAsync(request.Id) 
                        ?? throw new Exception($"Error: {nameof(DeleteStationCommand)}-no_data_found");
            _unitOfWork.StationRepository.SoftRemove(station);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}