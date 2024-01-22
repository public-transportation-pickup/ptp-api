using AutoMapper;
using Microsoft.Extensions.Logging;

namespace PTP.Application.Features;
public  class BaseBussiness<T> where T : class
{
    protected  readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly ILogger<T> _logger;
    public BaseBussiness(IUnitOfWork unitOfWork, IMapper mapper, ILogger<T> logger)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
}