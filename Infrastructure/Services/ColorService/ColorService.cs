using System.Net;
using Domain.Dtos.ColorDtos;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Color = Domain.Entities.Color;

namespace Infrastructure.Services.ColorService;

public class ColorService(ApplicationContext context) : IColorService
{
    public async Task<PagedResponse<List<GetColorDto>>> GetColors(ColorFilter filter)
    {
        try
        {
            var colors = context.Colors.AsQueryable();
            if (!string.IsNullOrEmpty(filter.ColorName))
                colors = colors.Where(c => c.ColorName.ToLower().Contains(filter.ColorName.ToLower()));
            var result = await colors.Select(c => new GetColorDto()
            {
                Id = c.Id,
                ColorName = c.ColorName
            }).AsNoTracking().ToListAsync();
            var totalRecord = colors.Count();
            return new PagedResponse<List<GetColorDto>>(result, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetColorDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetColorDto>> GetColorById(int id)
    {
        try
        {
            var color = await context.Colors.Select(c => new GetColorDto()
            {
                Id = c.Id,
                ColorName = c.ColorName
            }).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            return color == null ? new Response<GetColorDto>(HttpStatusCode.NotFound, "Color not found!") : new Response<GetColorDto>(color);
        }
        catch (Exception e)
        {
            return new Response<GetColorDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> AddColor(AddColorDto addColor)
    {
        try
        {
            var color = new Color()
            {
                ColorName = addColor.ColorName
            };
            await context.Colors.AddAsync(color);
            await context.SaveChangesAsync();
            return new Response<int>(color.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> UpdateColor(UpdateColorDto updateColor)
    {
        try
        {
            var color = new Color()
            {
                Id = updateColor.Id,
                ColorName = updateColor.ColorName
            };
            context.Colors.Update(color);
            await context.SaveChangesAsync();
            return new Response<int>(color.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteColor(int id)
    {
        try
        {
            var color = await context.Colors.FindAsync(id);
            if (color == null) return new Response<bool>(HttpStatusCode.NotFound, "Color not found!");
            context.Colors.Remove(color);
            await context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}