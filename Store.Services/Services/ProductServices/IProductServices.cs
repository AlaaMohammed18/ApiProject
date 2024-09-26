using Store.Services.Services.ProductServices.Dto;

namespace Store.Services.Services.ProductServices
{
    public interface IProductServices
    {
        Task<ProductDetailsDto> GetProductByIdAsync(int? ProductId);
        Task<IReadOnlyList<ProductDetailsDto>> GetAllProductsAsync();
        Task<IReadOnlyList<BrandsTypesDetailsDto>> GetAllBrandsAsync();
        Task<IReadOnlyList<BrandsTypesDetailsDto>> GetAllTypesAsync();
    }
}
