using System.Net.Http.Headers;
using FluentValidation;
using MediatR;
using PTP.Application.Features.Users.Queries;
using PTP.Application.ViewModels.Users;
using PTP.Domain.Entities;
using PTP.Domain.Enums;

namespace PTP.Application.Features.Users.Commands;
public class CreateUserCommand : IRequest<UserViewModel>
{
    public UserCreateModel Model { get; set; } = default!;
    public class CommandValidation : AbstractValidator<CreateUserCommand>
    {
        public CommandValidation()
        {
            RuleFor(x => x.Model.Email).NotNull().NotEmpty()
            .EmailAddress()
            .WithMessage($"Email not valid");
            RuleFor(x => x.Model.FullName).NotNull().NotEmpty().WithMessage($"Full Name is not valid");

        }
    }
    public class CommandHandler : IRequestHandler<CreateUserCommand, UserViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public CommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<UserViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.Mapper.Map<User>(request.Model);
            var isDup = await _unitOfWork.UserRepository.WhereAsync(x => x.Email!.ToLower() == request.Model.Email!.ToLower() ||
                x.PhoneNumber!.ToLower() == request.Model.PhoneNumber!.ToLower());
                if(isDup.Count() > 0) 
                throw new Exception($"Error: {nameof(CreateUserCommand)}_email or phone is duplicate!");
            request.Model.RoleName = request.Model.RoleName ?? nameof(RoleEnum.Customer);
            var role = await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.Name.ToLower() == request.Model.RoleName.ToLower())
                ?? throw new Exception($"Error: {nameof(CreateUserCommand)}_no_role_found: role: {request.Model.RoleName}");
            user.RoleId = role.Id;
            await _unitOfWork.UserRepository.AddAsync(user);
            if (await _unitOfWork.SaveChangesAsync())
            {
                return await _mediator.Send(new GetUserByIdQuery { Id = user.Id }, cancellationToken);
            }
            else
            {
                throw new Exception($"Error: {nameof(CreateUserCommand)}_Save Change Failed!");
            }
        }
    }
}