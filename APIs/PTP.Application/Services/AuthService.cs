using Firebase.Auth;
using PTP.Application.Services.Interfaces;
using PTP.Application.ViewModels.Users;

namespace PTP.Application.Services;
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppSettings _appSettings;
    private readonly IJWTTokenGenerator _jwtTokenGenerator;
    public AuthService(IUnitOfWork unitOfWork, AppSettings appSettings, IJWTTokenGenerator jWTTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _appSettings = appSettings;
        _jwtTokenGenerator = jWTTokenGenerator;
    }

    public async Task<LoginResponseModel> LoginAsync(string token, string role)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey: _appSettings.FirebaseSettings.ApiKeY));
        var user = await auth.GetUserAsync(token) ??
                    throw new Exception($"Error at: {nameof(IAuthService)}_ User not exist on firebase authentication");
        var userInDb = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == user.Email, x => x.Role);
        if (userInDb is not null)
        {
            // TODO: Gen token rồi trả Token system
            return new LoginResponseModel
            {
                Token = _jwtTokenGenerator.GenerateToken(user: userInDb, role: userInDb.Role.Name),
                User = _unitOfWork.Mapper.Map<UserViewModel>(userInDb)
            };
        }
        else
        {
            var roleInDb = await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.Name.ToLower() == role.ToLower())
                        ?? throw new Exception($"Error: {nameof(AuthService)}_ Role Not found: rolename: {role}");
            // TODO: Insert Db
            PTP.Domain.Entities.User newUser = new()
            {
                FullName = $"{user.LastName.ToUpper()} {user.FirstName.ToUpper()}",
                PhoneNumber = string.Empty,
                Id = Guid.NewGuid(),
                Email = user.Email,
                RoleId = roleInDb.Id,
                Role = roleInDb
            };
            await _unitOfWork.UserRepository.AddAsync(newUser);
            if (await _unitOfWork.SaveChangesAsync())
            {
                return new LoginResponseModel
                {
                    Token = _jwtTokenGenerator.GenerateToken(newUser, roleInDb.Name),
                    User = _unitOfWork.Mapper.Map<UserViewModel>(newUser)
                };
            }
            else throw new Exception($"Error: {nameof(AuthService)}_{nameof(LoginAsync)}: SaveChange new User Failed");


        }
    }
}