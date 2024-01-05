using System.Data;
using AutoMapper;
using FluentValidation;
using MediatR;
using PTP.Application.ViewModels;
using PTP.Domain.Entities;

namespace PTP.Application.Features.Roles.Queries.Commands;
public class RoleQuery : IRequest<IEnumerable<RoleViewModel>> 
{
   public class QueryValidation : AbstractValidator<RoleQuery>
   {
        public QueryValidation()
        {
            
        }
   }

    public class QueryHandler : IRequestHandler<RoleQuery, IEnumerable<RoleViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RoleViewModel>> Handle(RoleQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<RoleViewModel>>(await _unitOfWork.RoleRepository.GetAllAsync());
        }
    }
}