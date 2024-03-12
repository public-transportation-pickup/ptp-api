using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PTP.Application.Commons;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using PTP.Application.IntergrationServices.Interfaces;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Stores;
using PTP.Domain.Entities;
using PTP.Domain.Globals;

namespace PTP.Application.Features.Stores.Commands;
public class DeleteStoreCommand:IRequest<bool>
{
    public Guid Id{get;set;}
    public class CommmandValidation : AbstractValidator<DeleteStoreCommand>
    {
        public CommmandValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            
        }
    }
    
    public class CommandHandler : IRequestHandler<DeleteStoreCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public CommandHandler(IUnitOfWork unitOfWork,ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(DeleteStoreCommand request, CancellationToken cancellationToken)
        {
            //Remove From Cache
            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            await _cacheService.RemoveAsync(CacheKey.STORE+request.Id);

            var store = await _unitOfWork.StoreRepository.GetByIdAsync(request.Id);
            if(store is null ) throw new NotFoundException($"Store with Id-{request.Id} is not exist!");
            _unitOfWork.StoreRepository.SoftRemove(store);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}