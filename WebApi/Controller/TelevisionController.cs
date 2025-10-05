// using System.ComponentModel.DataAnnotations;
// using System.Net;
// using Domain.Dtos.TelevisionDtos;
// using Domain.Response;
// using Infrastructure.Services.TelevisionService;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace WebApi.Controller;
//
// public class TelevisionController(ITelevisionService service) : BaseController
// {
//     [HttpGet("get-televisions"), AllowAnonymous]
//     public async Task<IActionResult> GetTelevisions()
//     {
//         var result = await service.GetTelevisions();
//         return StatusCode(result.StatusCode, result);
//     }
//
//     [HttpGet("get-television-by-id"), AllowAnonymous]
//     public async Task<IActionResult> GetTelevisionById([Required]int id)
//     {
//         if (ModelState.IsValid)
//         {
//             var result = await service.GetTelevisionById(id);
//             return StatusCode(result.StatusCode, result);
//         }
//
//         var response = new Response<GetTelevisionDto>(HttpStatusCode.BadRequest, ModelStateErrors());
//         return StatusCode(response.StatusCode, response);
//     }
//
//     [HttpPost("add-television")]
//     public async Task<IActionResult> AddTelevision(AddTelevisionDto addTelevision)
//     {
//         if (ModelState.IsValid)
//         {
//             var result = await service.AddTelevision(addTelevision);
//             return StatusCode(result.StatusCode, result);
//         }
//
//         var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
//         return StatusCode(response.StatusCode, response);
//     }
//
//     [HttpPut("update-television")]
//     public async Task<IActionResult> UpdateTelevision(UpdateTelevisionDto updateTelevision)
//     {
//         if (ModelState.IsValid)
//         {
//             var result = await service.UpdateTelevision(updateTelevision);
//             return StatusCode(result.StatusCode, result);
//         }
//
//         var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
//         return StatusCode(response.StatusCode, response);
//     }
//
//     [HttpDelete("delete-television")]
//     public async Task<IActionResult> DeleteTelevision([Required]int id)
//     {
//         if (ModelState.IsValid)
//         {
//             var result = await service.DeleteTelevision(id);
//             return StatusCode(result.StatusCode, result);
//         }
//
//         var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
//         return StatusCode(response.StatusCode, response);
//     }
// }