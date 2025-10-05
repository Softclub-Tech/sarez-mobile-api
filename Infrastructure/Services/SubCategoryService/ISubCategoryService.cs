using Domain.Dtos.SubCategoryDtos;
using Domain.Response;

namespace Infrastructure.Services.SubCategoryService;

public interface ISubCategoryService
{
    Task<Response<List<GetSubCategoryDto>>> GetSubCategories();
    Task<Response<GetSubCategoryDto>> GetSubCategoryById(int id);
    Task<Response<int>> AddSubCategory(AddSubCategoryDto addSubCategory);
    Task<Response<int>> UpdateSubCategory(UpdateSubCategoryDto updateSubCategory);
    Task<Response<bool>> DeleteSubCategory(int id);
}