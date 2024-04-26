using PTP.Application.Services.Interfaces;
using PTP.Domain.Entities;
using PTP.Domain.Enums;
using PTP.Domain.Globals;

namespace PTP.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public ProductService(IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task UpdateProduct()
    {
        var productMenus = await _unitOfWork.ProductInMenuRepository.GetAllAsync();
        for (int i = 0; i < productMenus.Count; i++)
        {
            productMenus[i].QuantityUsed = 0;
            if (productMenus[i].Status == ProductInMenuStatusEnum.InActive.ToString())
            {
                productMenus[i].Status = ProductInMenuStatusEnum.Active.ToString();
            }
        }
        var products = await _unitOfWork.ProductRepository.WhereAsync(x => x.Status == ProductInMenuStatusEnum.InActive.ToString());
        for (int i = 0; i < products.Count; i++)
        {
            products[i].Status = ProductInMenuStatusEnum.Active.ToString();
        }

        _unitOfWork.ProductInMenuRepository.UpdateRange(productMenus);
        _unitOfWork.ProductRepository.UpdateRange(products);

        if (await _unitOfWork.SaveChangesAsync())
        {
            if (!_cacheService.IsConnected()) throw new Exception("Redis Server is not connected!");
            await _cacheService.RemoveByPrefixAsync<ProductInMenu>(CacheKey.PRODUCTMENU);
            await _cacheService.RemoveByPrefixAsync<Product>(CacheKey.PRODUCT);
        }
    }
}
