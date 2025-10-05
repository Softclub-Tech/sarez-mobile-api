using System.Net;
using Domain.Dtos.CartDTOs;
using Domain.Dtos.ProductDtos;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.CartService;

public class CartService(ApplicationContext context, IHttpContextAccessor httpContextAccessor) : ICartService
{
    private readonly string _userId = httpContextAccessor.HttpContext!.User.Claims.First(x => x.Type == "sid").Value;
    public async Task<Response<List<GetCartDto>>> GetProductsFromCart(string userId)
    {
        try
        {
            var cart = await context.Users.Where(x => x.Id == userId).Select(x => new GetCartDto()
            {
                ProductsInCart = x.Carts.Select(c => new GetProductsInCartDto()
                {
                    Id = c.Id,
                    Quantity = c.Quantity,
                    Product = new GetProductsDto()
                    {
                        Id = c.Product.Id,
                        ProductName = c.Product.ProductName,
                        Image = c.Product.ProductImages.Select(i => i.ImageName).First(),
                        Quantity = c.Product.Quantity,
                        Color = c.Product.Color.ColorName,
                        Price = c.Product.Price,
                        HasDiscount = c.Product.HasDiscountPrice,
                        DiscountPrice = c.Product.DiscountPrice
                    }
                }).ToList(),
                TotalProducts = x.Carts.Sum(p => p.Quantity),
                TotalPrice = x.Carts.Sum(p => p.Product.Price * p.Quantity),
                TotalDiscountPrice = x.Carts.Sum(p => p.Product.DiscountPrice * p.Quantity)
            }).AsNoTracking().ToListAsync();
            return new Response<List<GetCartDto>>(cart);
        }
        catch (Exception e)
        {
            return new Response<List<GetCartDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> AddProductToCart(int id, string userId)
    {
        try
        {
            var existProduct = await context.Carts.AsNoTracking()
                .AnyAsync(x => x.ProductId == id && x.ApplicationUserId == userId);
            if (existProduct)
                return new Response<string>(HttpStatusCode.BadRequest, "This product already exist in your cart!");
            var product = await context.Products.Where(p => p.Id == id).AsNoTracking().FirstOrDefaultAsync();
            if (product == null)
                return new Response<string>(HttpStatusCode.NotFound, "Product not found!");
            var productToCart = new Cart()
            {
                ProductId = product.Id,
                Quantity = 1,
                ApplicationUserId = userId,
                DateCreated = DateTimeOffset.UtcNow
            };
            await context.Carts.AddAsync(productToCart);
            await context.SaveChangesAsync();
            return new Response<string>("Product successfully added to cart");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> IncreaseProductInCart(int id)
    {
        try
        {
            var product = await context.Carts.FindAsync(id);
            if (product == null) return new Response<string>(HttpStatusCode.NotFound, "Product not found in cart!");
            product.Quantity++;
            if (product.Quantity > await context.Products.Where(x => x.Id == product.ProductId).Select(x => x.Quantity)
                    .FirstOrDefaultAsync())
                return new Response<string>(HttpStatusCode.BadRequest,
                    "The specified quantity exceeds quantity of product in stock!");
            await context.SaveChangesAsync();
            return new Response<string>("Successfully");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> ReduceProductInCart(int id)
    {
        try
        {
            var product = await context.Carts.FindAsync(id);
            if (product == null) return new Response<string>(HttpStatusCode.NotFound, "Product not found in cart!");
            product.Quantity--;
            if (product.Quantity == 0)
                return new Response<string>(HttpStatusCode.BadRequest,
                    "The number of product in the cart cannot be zero.");
            await context.SaveChangesAsync();
            return new Response<string>("Successfully");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> DeleteProductFromCart(int id)
    {
        try
        {
            var product = await context.Carts.FindAsync(id);
            if (product == null) return new Response<string>("Product not found in cart!");
            context.Carts.Remove(product);
            await context.SaveChangesAsync();
            return new Response<string>("Successfully");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> ClearCart()
    {
        try
        {
            var productsInCart = await context.Carts.Where(x => x.ApplicationUserId == _userId).ToListAsync();
            if (productsInCart.Count == 0)
                return new Response<string>(HttpStatusCode.BadRequest, "Your cart already clear.");
            context.Carts.RemoveRange(productsInCart);
            await context.SaveChangesAsync();
            return new Response<string>("Successfully.");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}