using Microsoft.AspNetCore.Mvc;
using PTP.Application;
using PTP.Application.Utilities;

namespace PTP.WebAPI.Controllers;
public class RolesController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    public RolesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    
    }
    
    /* 
        This only endpoint for testing
    */
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _unitOfWork.RoleRepository.GetAllAsync());
    }

}