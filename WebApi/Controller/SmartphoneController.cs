// using System.ComponentModel.DataAnnotations;
// using System.Net;
// using Domain.Dtos.SmartphoneDtos;
// using Domain.Response;
// using Infrastructure.Services.SmartphoneService;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace WebApi.Controller;
//
// public class SmartphoneController(ISmartphoneService service) : BaseController
// {
//     [HttpGet("get-smartphones"), AllowAnonymous]
//     public async Task<IActionResult> GetSmartphones()
//     {
//         var result = await service.GetSmartphones();
//         return StatusCode(result.StatusCode, result);
//     }
//
//     // [HttpGet("get-smartphone"), AllowAnonymous]
//     // public async Task<IActionResult> GetSmartphoneById([Required]int id)
//     // {
//     //     if (ModelState.IsValid)
//     //     {
//     //         var result = await service.GetSmartphoneById(id);
//     //         return StatusCode(result.StatusCode, result);
//     //     }
//     //
//     //     var response = new Response<GetSmartphoneDto>(HttpStatusCode.BadRequest, ModelStateErrors());
//     //     return StatusCode(response.StatusCode, response);
//     // }
//
//     [HttpPost("add-smartphone")]
//     public async Task<IActionResult> AddSmartphone([FromForm]AddSmartphoneDto addSmartphone)
//     {
//         if (ModelState.IsValid)
//         {
//             var userId = User.Claims.FirstOrDefault(x => x.Type == "sid")!.Value;
//             var result = await service.AddSmartphone(addSmartphone, userId);
//             return StatusCode(result.StatusCode, result);
//         }
//
//         var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
//         return StatusCode(response.StatusCode, response);
//     }
//
//     [HttpPut("update-smartphone")]
//     public async Task<IActionResult> UpdateSmartphone(UpdateSmartphoneDto updateSmartphone)
//     {
//         if (ModelState.IsValid)
//         {
//             var result = await service.UpdateSmartphone(updateSmartphone);
//             return StatusCode(result.StatusCode, Response);
//         }
//
//         var response = new Response<int>(HttpStatusCode.BadRequest, ModelStateErrors());
//         return StatusCode(response.StatusCode, Response);
//     }
//
//     [HttpDelete("delete-smartphone")]
//     public async Task<IActionResult> DeleteSmartphone([Required]int id)
//     {
//         if (ModelState.IsValid)
//         {
//             var result = await service.DeleteSmartphone(id);
//             return StatusCode(result.StatusCode, result);
//         }
//
//         var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
//         return StatusCode(response.StatusCode, response);
//     }
// }