using System.Net;
using Domain.Dtos.CategoryDtos;
using Domain.Dtos.SubCategoryDtos;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.CategoryService;

public class CategoryService(ApplicationContext context, IFileService fileService) : ICategoryService
{
    public async Task<Response<List<GetCategoryDto>>> GetCategories()
    {
        try
        {
            var categories = await context.Categories.Select(c => new GetCategoryDto()
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                CategoryImage = c.CategoryImage,
                SubCategories = c.SubCategories.Select(s => new GetSubCategoryDto()
                {
                    Id = s.Id,
                    SubCategoryName = s.SubCategoryName
                }).ToList()
            }).AsNoTracking().ToListAsync();
            return new Response<List<GetCategoryDto>>(categories);
        }
        catch (Exception e)
        {
            return new Response<List<GetCategoryDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetCategoryDto>> GetCategoryById(int id)
    {
        try
        {
            var category = await context.Categories.Select(c => new GetCategoryDto()
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                CategoryImage = c.CategoryImage,
                SubCategories = c.SubCategories.Select(s => new GetSubCategoryDto()
                {
                    Id = s.Id,
                    SubCategoryName = s.SubCategoryName
                }).ToList()
            }).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return new Response<GetCategoryDto>(HttpStatusCode.BadRequest, "Category not found!");
            return new Response<GetCategoryDto>(category);
        }
        catch (Exception e)
        {
            return new Response<GetCategoryDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> AddCategory(AddCategoryDto addCategory)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x =>
                x.CategoryName.Trim().ToLower() == addCategory.CategoryName.Trim().ToLower());
            if (category != null)
                return new Response<int>(HttpStatusCode.BadRequest, "Category with name already exists");

            var imageName = await fileService.CreateFile(addCategory.CategoryImage);

            var newCategory = new Category
            {
                CategoryName = addCategory.CategoryName,
                CategoryImage = imageName.Data!
            };

            await context.Categories.AddAsync(newCategory);
            await context.SaveChangesAsync();

            return new Response<int>(newCategory.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> UpdateCategory(UpdateCategoryDto updateCategory)
    {
        try
        {
            var category = await context.Categories.FindAsync(updateCategory.Id);
            if (category == null)
                return new Response<int>(HttpStatusCode.NotFound, "Category not found!");

            if(category.CategoryName.Trim().ToLower() != updateCategory.CategoryName.Trim().ToLower())
            {
                var categoryByName = await context.Categories.FirstOrDefaultAsync(x =>
                x.CategoryName.Trim().ToLower() == updateCategory.CategoryName.Trim().ToLower());
                if (categoryByName != null)
                    return new Response<int>(HttpStatusCode.BadRequest, "Category with name already exists");
            }

            fileService.DeleteFile(category.CategoryImage);

            var imageName = await fileService.CreateFile(updateCategory.CategoryImage);

            category.CategoryName = updateCategory.CategoryName;
            category.CategoryImage = imageName.Data!;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new Response<int>(category.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteCategory(int id)
    {
        try
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
                return new Response<bool>(HttpStatusCode.BadRequest, "Category not found!");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            fileService.DeleteFile(category.CategoryImage);

            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}