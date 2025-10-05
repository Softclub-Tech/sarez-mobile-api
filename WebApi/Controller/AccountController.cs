using System.Net;
using Domain.Dtos.AccountDtos;
using Domain.Response;
using Infrastructure.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

public class AccountController(IAccountService service) : BaseController
{
    [HttpPost("register"), AllowAnonymous]
    public async Task<IActionResult> Register([FromBody]RegisterDto model)
    {
        if (ModelState.IsValid)
        {
            var result = await service.Register(model);

            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<string>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody]LoginDto model)
    {
        if (ModelState.IsValid)
        {
            var result = await service.Login(model);

            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<string>(HttpStatusCode.BadRequest, ModelStateErrors());

        return StatusCode(response.StatusCode, response);
    }
}