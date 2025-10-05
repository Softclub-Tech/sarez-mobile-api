using Domain.Dtos.BrandDtos;
using Domain.Filters;
using Domain.Response;

namespace Infrastructure.Services.BrandService;

public interface IBrandService
{
    Task<PagedResponse<List<GetBrandDto>>> GetBrands(BrandFilter filter);
    Task<Response<GetBrandDto>> GetBrandById(int id);
    Task<Response<int>> AddBrand(AddBrandDto addBrand);
    Task<Response<int>> UpdateBrand(UpdateBrandDto addBrand);
    Task<Response<bool>> DeleteBrand(int id);
}