using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Dtos.CategoryDtos;
using Domain.Response;
using Infrastructure.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

public class CategoryController(ICategoryService service) : BaseController
{
    [HttpGet("get-categories"), AllowAnonymous]
    public async Task<IActionResult> GetCategories()
    {
        var result = await service.GetCategories();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-category-by-id"), AllowAnonymous]
    public async Task<IActionResult> GetCategoryById([Required]int id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.GetCategoryById(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<GetCategoryDto>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("add-category")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> AddCategory([FromForm]AddCategoryDto addCategory)
    {
        if (ModelState.IsValid)
        {
            var result = await service.AddCategory(addCategory);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("update-category")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> UpdateCategory([FromForm]UpdateCategoryDto updateCategory)
    {
        if (ModelState.IsValid)
        {
            var result = await service.UpdateCategory(updateCategory);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-category")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> DeleteCategory([Required]int id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.DeleteCategory(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}