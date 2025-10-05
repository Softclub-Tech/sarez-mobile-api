using Domain.Dtos.CategoryDtos;
using Domain.Response;

namespace Infrastructure.Services.CategoryService;

public interface ICategoryService
{
    Task<Response<List<GetCategoryDto>>> GetCategories();
    Task<Response<GetCategoryDto>> GetCategoryById(int id);
    Task<Response<int>> AddCategory(AddCategoryDto addCategory);
    Task<Response<int>> UpdateCategory(UpdateCategoryDto updateCategory);
    Task<Response<bool>> DeleteCategory(int id);
}