using MediatR;
using Microsoft.Identity.Client;

namespace PTP.Application.Features.Stations.Commands;
public class AddStoreIntoStationCommand : IRequest<bool>
{
    public Guid StoreId { get; set; }
    public Guid StationId { get; set; }
    public class CommandHandler : IRequestHandler<AddStoreIntoStationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(AddStoreIntoStationCommand request, CancellationToken cancellationToken)
        {
            var station = await _unitOfWork.StationRepository.GetByIdAsync(request.StationId);
            var store = await _unitOfWork.StoreRepository.GetByIdAsync(request.StoreId);
            if(store is null || station is null)
            {
                throw new Exception($"Error: {nameof(AddStoreIntoStationCommand)}-no_data_found");
            }
            if (station.StoreId is null || station.StoreId == Guid.Empty)
            {
                station.StoreId = store.Id;
                _unitOfWork.StationRepository.Update(station);
                return await _unitOfWork.SaveChangesAsync();
            }
            else throw new Exception($"Error: {nameof(AddStoreIntoStationCommand)}_Station has store register already");

        }
    }
}