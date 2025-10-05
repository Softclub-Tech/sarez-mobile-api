using Domain.Response;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.FileService;

public interface IFileService
{
    Task<Response<string>> CreateFile(IFormFile file);
    Task<Response<string>> UpdateFile(IFormFile newFile, string oldFile);
    Response<bool> DeleteFile(string file);
}