using System.Net;
using Domain.Dtos.SmartphoneDtos;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Services.ProductService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.SmartphoneService;

public class SmartphoneService(ApplicationContext context, IProductService productService) : ISmartphoneService
{
    public async Task<Response<List<GetSmartphoneDto>>> GetSmartphones()
    {
        try
        {
            var smartphones = await context.Smartphones.Select(s => new GetSmartphoneDto()
            {
                Id = s.Id,
                Model = s.Model,
                Os = s.Os,
                Communication = s.Communication,
                Processor = s.Processor,
                ProcessorFrequency = s.ProcessorFrequency,
                NumberOfCores = s.NumberOfCores,
                VideoProcessor = s.VideoProcessor,
                AspectRatio = s.AspectRatio,
                DisplayType = s.DisplayType,
                DisplayResolution = s.DisplayResolution,
                PixelPerInch = s.PixelPerInch,
                ScreenRefreshRate = s.ScreenRefreshRate,
                Diagonal = s.Diagonal,
                SimCard = s.SimCard,
                Ram = s.Ram,
                Rom = s.Rom
            }).ToListAsync();
            return new Response<List<GetSmartphoneDto>>(smartphones);
        }
        catch (Exception e)
        {
            return new Response<List<GetSmartphoneDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetSmartphoneDto>> GetSmartphoneById(int id)
    {
        try
        {
            var smartphone = await context.Smartphones.Select(s => new GetSmartphoneDto()
            {
                Id = s.Id,
                Model = s.Model,
                Os = s.Os,
                Communication = s.Communication,
                Processor = s.Processor,
                ProcessorFrequency = s.ProcessorFrequency,
                NumberOfCores = s.NumberOfCores,
                VideoProcessor = s.VideoProcessor,
                AspectRatio = s.AspectRatio,
                DisplayType = s.DisplayType,
                DisplayResolution = s.DisplayResolution,
                PixelPerInch = s.PixelPerInch,
                ScreenRefreshRate = s.ScreenRefreshRate,
                Diagonal = s.Diagonal,
                SimCard = s.SimCard,
                Ram = s.Ram,
                Rom = s.Rom,
            }).FirstOrDefaultAsync(s => s.Id == id);
            if (smartphone == null)
                return new Response<GetSmartphoneDto>(HttpStatusCode.NotFound, "Smartphone not found!");
            return new Response<GetSmartphoneDto>(smartphone);
        }
        catch (Exception e)
        {
            return new Response<GetSmartphoneDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> AddSmartphone(AddSmartphoneDto addSmartphone, string userId)
    {
        try
        {
            var productId = await productService.AddProduct(addSmartphone.AddProductDto, userId);
            var smartphone = new Smartphone()
            {
                Model = addSmartphone.Model,
                Os = addSmartphone.Os,
                Communication = addSmartphone.Communication,
                Processor = addSmartphone.Processor,
                ProcessorFrequency = addSmartphone.ProcessorFrequency,
                NumberOfCores = addSmartphone.NumberOfCores,
                VideoProcessor = addSmartphone.VideoProcessor,
                AspectRatio = addSmartphone.AspectRatio,
                DisplayType = addSmartphone.DisplayType,
                DisplayResolution = addSmartphone.DisplayResolution,
                PixelPerInch = addSmartphone.PixelPerInch,
                ScreenRefreshRate = addSmartphone.ScreenRefreshRate,
                Diagonal = addSmartphone.Diagonal,
                SimCard = addSmartphone.SimCard,
                Ram = addSmartphone.Ram,
                Rom = addSmartphone.Rom
            };
            await context.Smartphones.AddAsync(smartphone);
            await context.SaveChangesAsync();
            var product = await context.Products.FindAsync(productId.Data);
            product!.SmartphoneId = smartphone.Id;
            await context.SaveChangesAsync();
            return new Response<int>(productId.Data);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> UpdateSmartphone(UpdateSmartphoneDto updateSmartphone)
    {
        try
        {
            var smartphone = new Smartphone()
            {
                Id = updateSmartphone.Id,
                Model = updateSmartphone.Model,
                Os = updateSmartphone.Os,
                Communication = updateSmartphone.Communication,
                Processor = updateSmartphone.Processor,
                ProcessorFrequency = updateSmartphone.ProcessorFrequency,
                NumberOfCores = updateSmartphone.NumberOfCores,
                VideoProcessor = updateSmartphone.VideoProcessor,
                AspectRatio = updateSmartphone.AspectRatio,
                DisplayType = updateSmartphone.DisplayType,
                DisplayResolution = updateSmartphone.DisplayResolution,
                PixelPerInch = updateSmartphone.PixelPerInch,
                ScreenRefreshRate = updateSmartphone.ScreenRefreshRate,
                Diagonal = updateSmartphone.Diagonal,
                SimCard = updateSmartphone.SimCard,
                Ram = updateSmartphone.Ram,
                Rom = updateSmartphone.Rom
            }; 
            context.Smartphones.Update(smartphone);
            await context.SaveChangesAsync();
            return new Response<int>(smartphone.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteSmartphone(int id)
    {
        try
        {
            var smartphone = await context.Smartphones.FindAsync(id);
            if (smartphone == null) return new Response<bool>(HttpStatusCode.NotFound, "Smartphone not found!");
            context.Smartphones.Remove(smartphone);
            await context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}