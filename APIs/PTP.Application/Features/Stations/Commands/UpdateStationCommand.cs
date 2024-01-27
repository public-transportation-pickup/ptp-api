using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PTP.Application.ViewModels.Stations;

namespace PTP.Application.Features.Stations.Commands;
public class UpdateStationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public StationUpdateModel Model { get; set; } = default!;
    public class CommandValidation : AbstractValidator<UpdateStationCommand>
    {
        public CommandValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Model).NotNull();
        }
    }
    public class CommandHandler : IRequestHandler<UpdateStationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(UpdateStationCommand request, CancellationToken cancellationToken)
        {
            var station = await _unitOfWork.StationRepository.GetByIdAsync(request.Id)
                            ?? throw new Exception($"Error: {nameof(UpdateStationCommand)}-no_data_found");
            _unitOfWork.Mapper.Map(request.Model, station);
            _unitOfWork.StationRepository.Update(station);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}