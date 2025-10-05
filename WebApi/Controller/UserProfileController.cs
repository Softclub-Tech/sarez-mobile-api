using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Dtos.UserProfileDtos;
using Domain.Dtos.UserRoleDtos;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Services.UserProfileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

public class UserProfileController(IUserProfileService service) : BaseController
{
    [HttpGet("get-user-profiles")]
    public async Task<IActionResult> GetUserProfiles(UserProfileFilter filter)
    {
        var result = await service.GetUserProfiles(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-user-profile-by-id")]
    public async Task<IActionResult> GetUserProfileById([Required]string id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.GetUserProfileById(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<GetUserProfileDto>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("update-user-profile")]
    public async Task<IActionResult> UpdateUserProfile([FromForm] AddUserProfileDto addUserProfile)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "sid")!.Value;
            var result = await service.UpdateUserProfileDto(addUserProfile, userId);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-user")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> DeleteUser([Required]string id)
    {
        if (ModelState.IsValid)
        {
            var result = await service.DeleteUser(id);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("addrole-from-user")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> AddRoleFromUser(AddRoleFromUserDto addRoleFromUserDto)
    {
        if (ModelState.IsValid)
        {
            var result = await service.AddRoleFromUser(addRoleFromUserDto);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpDelete("remove-role-from-user")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> RemoveRoleFromUser(RemoveRoleFromUserDto roleFromUserDto)
    {
        if (ModelState.IsValid)
        {
            var result = await service.RemoveRoleFromUser(roleFromUserDto);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-user-roles")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public async Task<IActionResult> GetUserRoles()
    {
        var result = await service.GetUserRoles();
        return StatusCode(result.StatusCode, result);
    }
}