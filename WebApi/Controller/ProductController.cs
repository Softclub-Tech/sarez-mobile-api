using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Dtos.ImageDTOs;
using Domain.Dtos.ProductDtos;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

public class ProductController(IProductService service) : BaseController
{
    [HttpGet("get-products"), AllowAnonymous]
    public async Task<IActionResult> GetProducts(ProductFilter filter)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "sid")?.Value;
        var result = await service.GetProductPage(filter, userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-product-by-id"), AllowAnonymous]
    public async Task<IActionResult> GetProductById([Required]int id)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "sid")?.Value;
            var result = await service.GetProductById(id, userId);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<GetProductDto>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("add-product")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> AddProduct([FromForm]AddProductDto addProduct)
    {
        if (ModelState.IsValid)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "sid")!.Value;
            var result = await service.AddProduct(addProduct, user);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("update-product")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProduct)
    {
        if (ModelState.IsValid)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "sid")!.Value;
            var result = await service.UpdateProduct(updateProduct, user);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("add-image-to-product")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> AddImageToProduct([FromForm]AddImageToProductDto imageToProduct)
    {
        if (ModelState.IsValid)
        {
            var result = await service.AddImageToProduct(imageToProduct);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<string>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-image-from-product")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> DeleteImageFromProduct([Required]int imageId)
    {
        if (ModelState.IsValid)
        {
            var result = await service.DeleteImageFromProduct(imageId);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<string>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpDelete("delete-product")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> DeleteProduct([Required]int id)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.First(x => x.Type == "sid").Value;
            var result = await service.DeleteProduct(id, userId);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}