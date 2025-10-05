using System.Net;
using Domain.Dtos.SubCategoryDtos;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.SubCategoryService;

public class SubCategoryService(ApplicationContext context) : ISubCategoryService
{
    public async Task<Response<List<GetSubCategoryDto>>> GetSubCategories()
    {
        try
        {
            var subCategories = await context.SubCategories.Select(s => new GetSubCategoryDto
            {
                Id = s.Id,
                SubCategoryName = s.SubCategoryName,
                CategoryId = s.CategoryId,
            })
                .AsNoTracking()
                .ToListAsync();

            return new Response<List<GetSubCategoryDto>>(subCategories);
        }
        catch (Exception e)
        {
            return new Response<List<GetSubCategoryDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetSubCategoryDto>> GetSubCategoryById(int id)
    {
        try
        {
            var subCategory = await context.SubCategories.Select(s => new GetSubCategoryDto()
            {
                Id = s.Id,
                SubCategoryName = s.SubCategoryName,
                CategoryId = s.CategoryId,
            }).AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

            if (subCategory == null)
                return new Response<GetSubCategoryDto>(HttpStatusCode.NotFound, "SubCategory not found!");
            return new Response<GetSubCategoryDto>(subCategory);
        }
        catch (Exception e)
        {
            return new Response<GetSubCategoryDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> AddSubCategory(AddSubCategoryDto addSubCategory)
    {
        try
        {
            var subCategory = await context.SubCategories.FirstOrDefaultAsync(x =>
                x.SubCategoryName.Trim().ToLower() == addSubCategory.SubCategoryName.Trim().ToLower());
            if (subCategory != null)
                return new Response<int>(HttpStatusCode.BadRequest, "SubCategory with name already exists");

            var newSubCategory = new SubCategory()
            {
                CategoryId = addSubCategory.CategoryId,
                SubCategoryName = addSubCategory.SubCategoryName
            };

            await context.SubCategories.AddAsync(newSubCategory);
            await context.SaveChangesAsync();

            return new Response<int>(newSubCategory.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> UpdateSubCategory(UpdateSubCategoryDto updateSubCategory)
    {
        try
        {
            var subCategoryByName = await context.SubCategories.FirstOrDefaultAsync(x =>
                x.SubCategoryName.Trim().ToLower() == updateSubCategory.SubCategoryName.Trim().ToLower());
            if (subCategoryByName != null)
                return new Response<int>(HttpStatusCode.BadRequest, "SubCategory with name already exists");

            var subCategory = new SubCategory
            {
                Id = updateSubCategory.Id,
                CategoryId = updateSubCategory.CategoryId,
                SubCategoryName = updateSubCategory.SubCategoryName
            };

            context.SubCategories.Update(subCategory);
            await context.SaveChangesAsync();

            return new Response<int>(subCategory.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteSubCategory(int id)
    {
        try
        {
            var subCategory = await context.SubCategories.FindAsync(id);
            if (subCategory == null)
                return new Response<bool>(HttpStatusCode.NotFound, "SubCategory not found!");

            context.SubCategories.Remove(subCategory);
            await context.SaveChangesAsync();

            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}