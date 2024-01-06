using System.Data;
using AutoMapper;
using Dapper;
using FluentValidation;
using MediatR;
using PTP.Application.Data.Configuration;
using PTP.Application.ViewModels;
using PTP.Domain.Entities;


namespace PTP.Application.Features.Roles.Queries;
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
        private readonly IConnectionConfiguration _connection;
        private readonly IMapper _mapper;
        public QueryHandler(IConnectionConfiguration connection, IMapper mapper) 
        {
            _connection = connection;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RoleViewModel>> Handle(RoleQuery request, CancellationToken cancellationToken)
        {
            using var dbConnection = _connection.GetDbConnection();
            var result = await dbConnection.QueryAsync<Role>(@"SELECT * FROM Role");
            result = result ?? throw new Exception("No_Data_Found");
            return _mapper.Map<IEnumerable<RoleViewModel>>(result);
        }
    }
}