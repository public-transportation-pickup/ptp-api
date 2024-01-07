using System.Data;
using AutoMapper;
using Dapper;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using PTP.Application.Data.Configuration;
using PTP.Application.Services.Interfaces;
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
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        public QueryHandler(IConnectionConfiguration connection, IMapper mapper,ICacheService cacheService) 
        {
            _connection = connection;
            _cacheService = cacheService;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RoleViewModel>> Handle(RoleQuery request, CancellationToken cancellationToken)
        {
            var cachedValue = await _cacheService.GetByPrefixAsync<Role>("role");
            if(cachedValue!.Count>0)
            {
                return _mapper.Map<IEnumerable<RoleViewModel>>(cachedValue);
            }
            using var dbConnection = _connection.GetDbConnection();
            
            var result = await dbConnection.QueryAsync<Role>(@"SELECT * FROM Role");
            result = result ?? throw new Exception("No_Data_Found");
            await _cacheService.SetByPrefixAsync("role", result.ToList(), cancellationToken);
            return _mapper.Map<IEnumerable<RoleViewModel>>(result);
        }
    }
}