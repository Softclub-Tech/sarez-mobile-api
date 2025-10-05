using System.Net;
using Domain.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.FileService;

public class FileService(IWebHostEnvironment hostEnvironment) : IFileService
{
    public async Task<Response<string>> CreateFile(IFormFile file)
    {
        try
        {
            var fileName = string.Format($"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");
            var fullPath = Path.Combine(hostEnvironment.WebRootPath, "images", fileName);
            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new Response<string>(fileName);
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateFile(IFormFile newFile, string oldFile)
    {
        try
        {
            DeleteFile(oldFile);
            var fileName = await CreateFile(newFile);
            return new Response<string>(fileName.Data!);
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public Response<bool> DeleteFile(string file)
    {
        try
        {
            var fullPath = Path.Combine(hostEnvironment.WebRootPath, "images", file);
            File.Delete(fullPath);
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}