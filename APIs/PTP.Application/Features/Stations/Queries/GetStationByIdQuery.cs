using Dapper;
using FluentValidation;
using MediatR;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels.Stations;

namespace PTP.Application.Features.Stations.Queries;
public class GetStationByIdQuery : IRequest<StationViewModel?>
{
    public Guid Id { get; set; } = default!;
    public class QueryValidation : AbstractValidator<GetStationByIdQuery> 
    {
        public QueryValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class QueryHandler : IRequestHandler<GetStationByIdQuery, StationViewModel?>
    {
        private readonly IUnitOfWork unitOfWork;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<StationViewModel?> Handle(GetStationByIdQuery request, CancellationToken cancellationToken)
        {
            
            var result = await unitOfWork.StationRepository.GetByIdAsync(request.Id);
            return unitOfWork.Mapper.Map<StationViewModel>(result) ?? throw new Exception($"Error: {nameof(GetStationByIdQuery)}-no_data_found");

        }
    }
}