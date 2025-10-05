using Domain.Dtos.AccountDtos;
using Domain.Response;

namespace Infrastructure.Services.AccountService;

public interface IAccountService
{
    Task<Response<string>> Register(RegisterDto model);
    Task<Response<string>> Login(LoginDto model);
}