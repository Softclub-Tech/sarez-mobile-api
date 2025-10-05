using Domain.Dtos.ColorDtos;
using Domain.Filters;
using Domain.Response;

namespace Infrastructure.Services.ColorService;

public interface IColorService
{
    Task<PagedResponse<List<GetColorDto>>> GetColors(ColorFilter filter);
    Task<Response<GetColorDto>> GetColorById(int id);
    Task<Response<int>> AddColor(AddColorDto addColor);
    Task<Response<int>> UpdateColor(UpdateColorDto updateColor);
    Task<Response<bool>> DeleteColor(int id);
}