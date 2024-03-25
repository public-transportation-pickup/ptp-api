using FluentValidation;
using MediatR;
using PTP.Application.ViewModels.Users;

namespace PTP.Application.Features.Users.Commands;
public class UpdateUserCommand : IRequest
{
	public Guid Id { get; set; } = Guid.Empty;
	public UserUpdateModel Model { get; set; } = default!;
	public class CommandValidation : AbstractValidator<UpdateUserCommand>
	{
		public CommandValidation()
		{
			RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
			RuleFor(x => x.Model.FullName).NotNull().NotEmpty()
							.WithMessage("Name must not null or empty");

		}
	}
	public class CommandHandler : IRequestHandler<UpdateUserCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
		{
			if (request.Model.RoleName is null)
			{
				var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id, x => x.Role)
					   ?? throw new Exception($"Error: {nameof(UpdateUserCommand)}_no_user_found of Id: {request.Id}");
				_ = _unitOfWork.Mapper.Map(request.Model, user);
				_unitOfWork.UserRepository.Update(user);
				await _unitOfWork.SaveChangesAsync();
			}
			else
			{
				var role = await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.Name.ToLower() == request.Model.RoleName.ToLower())
								?? throw new Exception($"Error at {nameof(UpdateUserCommand)}_ role_not_found, roleName: {request.Model.RoleName}");
				var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id)
					   ?? throw new Exception($"Error: {nameof(UpdateUserCommand)}_no_user_found of Id: {request.Id}");
				user.Role = role;
				user.RoleId = role.Id;
				_ = _unitOfWork.Mapper.Map(request.Model, user);
				_unitOfWork.UserRepository.Update(user);
				await _unitOfWork.SaveChangesAsync();
			}

		}
	}
}