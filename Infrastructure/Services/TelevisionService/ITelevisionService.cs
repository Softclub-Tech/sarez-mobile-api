using Domain.Dtos.TelevisionDtos;
using Domain.Response;

namespace Infrastructure.Services.TelevisionService;

public interface ITelevisionService
{
    Task<Response<List<GetTelevisionDto>>> GetTelevisions();
    Task<Response<GetTelevisionDto>> GetTelevisionById(int id);
    Task<Response<int>> AddTelevision(AddTelevisionDto addTelevision);
    Task<Response<int>> UpdateTelevision(UpdateTelevisionDto updateTelevision);
    Task<Response<bool>> DeleteTelevision(int id);
}