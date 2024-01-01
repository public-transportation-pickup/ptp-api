using PTP.Application.Services.Interfaces;

namespace PTP.Application.Services;
public class CurrentTime : ICurrentTime
{
    public DateTime GetCurrentTime() => DateTime.UtcNow;                        
    
}