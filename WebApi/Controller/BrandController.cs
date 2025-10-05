using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Dtos.BrandDtos;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Services.BrandService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

public class BrandController(IBrandService service) : BaseController
{
    [HttpGet("get-brands"), AllowAnonymous]
    public async Task<IActionResult> GetBrands(BrandFilter filter)
    {
        var response = await service.GetBrands(filter);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-brand-by-id"), AllowAnonymous]
    public async Task<IActionResult> GetBrandById([Required]int id)
    {
        var response = await service.GetBrandById(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("add-brand")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> AddBrand(AddBrandDto addBrand)
    {
        if (ModelState.IsValid)
        {
            var result = await service.AddBrand(addBrand);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("update-brand")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> Update(UpdateBrandDto updateBrand)
    {
        if (ModelState.IsValid)
        {
            var result = await service.UpdateBrand(updateBrand);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-brand")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> DeleteBrand([Required]int id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.DeleteBrand(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}