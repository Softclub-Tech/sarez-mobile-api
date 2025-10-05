using Domain.Dtos.CartDTOs;
using Domain.Response;

namespace Infrastructure.Services.CartService;

public interface ICartService
{
    Task<Response<List<GetCartDto>>> GetProductsFromCart(string userId);
    Task<Response<string>> AddProductToCart(int id, string userId);
    Task<Response<string>> IncreaseProductInCart(int id);
    Task<Response<string>> ReduceProductInCart(int id);
    Task<Response<string>> DeleteProductFromCart(int id);
    Task<Response<string>> ClearCart();
}