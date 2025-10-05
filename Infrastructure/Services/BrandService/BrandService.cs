using System.Net;
using Domain.Dtos.BrandDtos;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Exception = System.Exception;

namespace Infrastructure.Services.BrandService;

public class BrandService(ApplicationContext context) : IBrandService
{
    public async Task<PagedResponse<List<GetBrandDto>>> GetBrands(BrandFilter filter)
    {
        try
        {
            var brands = context.Brands.AsQueryable();

            if (!string.IsNullOrEmpty(filter.BrandName))
                brands = brands.Where(b => b.BrandName.ToLower().Contains(filter.BrandName.ToLower()));

            if (filter.BrandId != 0)
                brands = brands.Where(b => b.Id == filter.BrandId);

            var result = await brands.Select(b => new GetBrandDto()
            {
                Id = b.Id,
                BrandName = b.BrandName
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).AsNoTracking().ToListAsync();

            var totalRecord = brands.Count();

            return new PagedResponse<List<GetBrandDto>>(result, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetBrandDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetBrandDto>> GetBrandById(int id)
    {
        try
        {
            var brand = await context.Brands.Select(b => new GetBrandDto()
            {
                Id = b.Id,
                BrandName = b.BrandName
            }).AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null) return new Response<GetBrandDto>(HttpStatusCode.BadRequest, "Brand not found!");
            return new Response<GetBrandDto>(brand);
        }
        catch (Exception e)
        {
            return new Response<GetBrandDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> AddBrand(AddBrandDto addBrand)
    {
        try
        {
            var existBrand = await context.Brands.AnyAsync(x =>
                x.BrandName.ToLower().Trim() == addBrand.BrandName.ToLower().Trim());
            if (existBrand)
            {
                return new Response<int>(HttpStatusCode.BadRequest, "This brand already exist!");
            }

            var brand = new Brand
            {
                BrandName = addBrand.BrandName
            };

            await context.Brands.AddAsync(brand);
            await context.SaveChangesAsync();

            return new Response<int>(brand.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> UpdateBrand(UpdateBrandDto updateBrand)
    {
        try
        {
            var brand = await context.Brands.FindAsync(updateBrand.Id);
            if (brand == null) return new Response<int>(HttpStatusCode.NotFound, "Brand not found!");
            brand.BrandName = updateBrand.BrandName;
            context.Brands.Update(brand);
            await context.SaveChangesAsync();
            return new Response<int>(brand.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteBrand(int id)
    {
        try
        {
            var brand = await context.Brands.FindAsync(id);
            if (brand == null) return new Response<bool>(HttpStatusCode.BadRequest, "Brand not found!");
            context.Brands.Remove(brand);
            await context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}