using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class ProductUnitOfWork : IProductUnitOfWork
{
    private readonly IProductService _productService;

    public ProductUnitOfWork(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ActionResponse<IEnumerable<Product>>> ComboAsync(string email, Guid id) => await _productService.ComboAsync(email, id);

    public async Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination, string email) => await _productService.GetAsync(pagination, email);

    public async Task<ActionResponse<Product>> GetAsync(Guid id) => await _productService.GetAsync(id);

    public async Task<ActionResponse<Product>> UpdateAsync(Product modelo) => await _productService.UpdateAsync(modelo);

    public async Task<ActionResponse<Product>> AddAsync(Product modelo, string email) => await _productService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _productService.DeleteAsync(id);
}