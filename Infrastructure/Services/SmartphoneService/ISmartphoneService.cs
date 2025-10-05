using Domain.Dtos.SmartphoneDtos;
using Domain.Response;

namespace Infrastructure.Services.SmartphoneService;

public interface ISmartphoneService
{
    Task<Response<List<GetSmartphoneDto>>> GetSmartphones();
    Task<Response<GetSmartphoneDto>> GetSmartphoneById(int id);
    Task<Response<int>> AddSmartphone(AddSmartphoneDto addSmartphone, string userId);
    Task<Response<int>> UpdateSmartphone(UpdateSmartphoneDto updateSmartphone);
    Task<Response<bool>> DeleteSmartphone(int id);
}