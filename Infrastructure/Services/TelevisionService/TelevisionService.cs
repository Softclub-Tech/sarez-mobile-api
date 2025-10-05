using System.Net;
using Domain.Dtos.TelevisionDtos;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.TelevisionService;

public class TelevisionService(ApplicationContext context) : ITelevisionService
{
    public async Task<Response<List<GetTelevisionDto>>> GetTelevisions()
    {
        try
        {
            var televisions = await context.Televisions.Select(t => new GetTelevisionDto()
            {
                Id = t.Id,
                SubCategory = t.SubCategory.SubCategoryName,
                Model = t.Model,
                ProductId = t.ProductId,
                Diagonal = t.Diagonal,
                DisplayResolution = t.DisplayResolution,
                NumberOfSpeaker = t.NumberOfSpeaker
            }).ToListAsync();
            return new Response<List<GetTelevisionDto>>(televisions);
        }
        catch (Exception e)
        {
            return new Response<List<GetTelevisionDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetTelevisionDto>> GetTelevisionById(int id)
    {
        try
        {
            var televisions = await context.Televisions.Select(t => new GetTelevisionDto()
            {
                Id = t.Id,
                SubCategory = t.SubCategory.SubCategoryName,
                Model = t.Model,
                ProductId = t.ProductId,
                Diagonal = t.Diagonal,
                DisplayResolution = t.DisplayResolution,
                NumberOfSpeaker = t.NumberOfSpeaker
            }).FirstOrDefaultAsync(t => t.Id == id);
            if (televisions == null)
                return new Response<GetTelevisionDto>(HttpStatusCode.NotFound, "Television not found!");
            return new Response<GetTelevisionDto>(televisions);
        }
        catch (Exception e)
        {
            return new Response<GetTelevisionDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> AddTelevision(AddTelevisionDto addTelevision)
    {
        try
        {
            var television = new Television()
            {
                SubCategoryId = addTelevision.SubCategoryId,
                ProductId = addTelevision.ProductId,
                Model = addTelevision.Model,
                Diagonal = addTelevision.Diagonal,
                DisplayResolution = addTelevision.DisplayResolution,
                NumberOfSpeaker = addTelevision.NumberOfSpeaker
            };
            await context.Televisions.AddAsync(television);
            await context.SaveChangesAsync();
            return new Response<int>(television.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> UpdateTelevision(UpdateTelevisionDto updateTelevision)
    {
        try
        {
            var television = new Television()
            {
                Id = updateTelevision.Id,
                SubCategoryId = updateTelevision.SubCategoryId,
                ProductId = updateTelevision.ProductId,
                Model = updateTelevision.Model,
                Diagonal = updateTelevision.Diagonal,
                DisplayResolution = updateTelevision.DisplayResolution,
                NumberOfSpeaker = updateTelevision.NumberOfSpeaker
            };
            context.Televisions.Update(television);
            await context.SaveChangesAsync();
            return new Response<int>(television.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteTelevision(int id)
    {
        try
        {
            var television = await context.Televisions.FindAsync(id);
            if (television == null) return new Response<bool>(HttpStatusCode.NotFound, "Television not found!");
            context.Televisions.Remove(television);
            await context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}