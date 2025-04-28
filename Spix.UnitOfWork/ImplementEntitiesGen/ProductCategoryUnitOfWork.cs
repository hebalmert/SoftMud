using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class ProductCategoryUnitOfWork : IProductCategoryUnitOfWork
{
    private readonly IProductCategoryService _productCategoryService;

    public ProductCategoryUnitOfWork(IProductCategoryService productCategoryService)
    {
        _productCategoryService = productCategoryService;
    }

    public async Task<ActionResponse<IEnumerable<ProductCategory>>> GetAsync(PaginationDTO pagination, string email) => await _productCategoryService.GetAsync(pagination, email);

    public async Task<ActionResponse<ProductCategory>> GetAsync(Guid id) => await _productCategoryService.GetAsync(id);

    public async Task<ActionResponse<ProductCategory>> UpdateAsync(ProductCategory modelo) => await _productCategoryService.UpdateAsync(modelo);

    public async Task<ActionResponse<ProductCategory>> AddAsync(ProductCategory modelo, string email) => await _productCategoryService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _productCategoryService.DeleteAsync(id);
}