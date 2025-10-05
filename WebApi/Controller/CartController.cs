using System.ComponentModel.DataAnnotations;
using Infrastructure.Services.CartService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

public class CartController(ICartService service) : BaseController
{
    [HttpGet("get-products-from-cart")]
    public async Task<IActionResult> GetProductsFromCart()
    {
        var userId = User.Claims.First(x => x.Type == "sid").Value;
        var result = await service.GetProductsFromCart(userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-product-to-cart")]
    public async Task<IActionResult> AddProductToCart([Required]int id)
    {
        var userId = User.Claims.First(x => x.Type == "sid").Value;
        var result = await service.AddProductToCart(id, userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("increase-product-in-cart")]
    public async Task<IActionResult> IncreaseProductInCart([Required]int id)
    {
        var result = await service.IncreaseProductInCart(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("reduce-product-in-cart")]
    public async Task<IActionResult> ReduceProductInCart([Required]int id)
    {
        var result = await service.ReduceProductInCart(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("delete-product-from-cart")]
    public async Task<IActionResult> DeleteProductFromCart([Required] int id)
    {
        var result = await service.DeleteProductFromCart(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("clear-cart")]
    public async Task<IActionResult> ClearCart()
    {
        var result = await service.ClearCart();
        return StatusCode(result.StatusCode, result);
    }
}