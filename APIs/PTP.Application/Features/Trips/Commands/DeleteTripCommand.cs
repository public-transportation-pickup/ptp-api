using MediatR;

namespace PTP.Application.Features.Trips.Commands;
public class DeleteTripCommand : IRequest<bool>
{
    public Guid Id { get; set; } = default;
    public class CommandHandler : IRequestHandler<DeleteTripCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteTripCommand request, CancellationToken cancellationToken)
        {
            var trip = await unitOfWork.TripRepository.GetByIdAsync(request.Id) ?? throw new Exception($"no_data_found at {nameof(DeleteTripCommand)}");
            unitOfWork.TripRepository.SoftRemove(trip);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}