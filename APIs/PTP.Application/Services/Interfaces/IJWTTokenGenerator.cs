using PTP.Domain.Entities;

namespace PTP.Application.Services.Interfaces;
public interface IJWTTokenGenerator
{
    string GenerateToken(User user, string role);
}