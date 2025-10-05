using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Dtos.ColorDtos;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Services.ColorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

public class ColorController(IColorService service) : BaseController
{
    [HttpGet("get-colors"), AllowAnonymous]
    public async Task<IActionResult> GetColors(ColorFilter filter)
    {
        var result = await service.GetColors(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-color-by-id"), AllowAnonymous]
    public async Task<IActionResult> GetColorById([Required]int id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.GetColorById(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<GetColorDto>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("add-color")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> AddColor(AddColorDto addColor)
    {
        if (ModelState.IsValid)
        {
            var result = await service.AddColor(addColor);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("update-color"), AllowAnonymous]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> UpdateColor(UpdateColorDto updateColor)
    {
        if (ModelState.IsValid)
        {
            var result = await service.UpdateColor(updateColor);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-color")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> DeleteColor([Required]int id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.DeleteColor(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}