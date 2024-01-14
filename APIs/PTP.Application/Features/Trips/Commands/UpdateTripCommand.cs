using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.ViewModels.Trips;

namespace PTP.Application.Features.Trips.Commands;
public class UpdateTripCommand : IRequest<bool>
{
    public Guid Id { get; set; } = default!;
    public TripUpdateModel Model { get; set; } = default!;
    public class CommandValidation : AbstractValidator<UpdateTripCommand>
    {
        public CommandValidation() 
        {
            
        }
    }
    public class CommandHandler : IRequestHandler<UpdateTripCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(UpdateTripCommand request, CancellationToken cancellationToken)
        {
            var trip = await unitOfWork.TripRepository.GetByIdAsync(request.Id);
            if (trip is not null)
            {
                unitOfWork.Mapper.Map(request.Model, trip);
                unitOfWork.TripRepository.Update(trip);
                return await unitOfWork.SaveChangesAsync();
            }
            else throw new 
            NotFoundException($"no_data_found at {nameof(UpdateTripCommand)}");
        }
    }
}