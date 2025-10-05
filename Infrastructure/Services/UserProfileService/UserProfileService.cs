using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Dtos.UserProfileDtos;
using Domain.Dtos.UserRoleDtos;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.UserProfileService;

public class UserProfileService(
    ApplicationContext context,
    UserManager<ApplicationUser> userManager,
    IFileService fileService) : IUserProfileService
{
    public async Task<PagedResponse<List<GetUserProfileDto>>> GetUserProfiles(UserProfileFilter filter)
    {
        try
        {
            var users = context.UserProfiles.AsQueryable();
            if (filter.UserName != null)
                users = users.Where(u => u.ApplicationUser.UserName!.ToLower().Contains(filter.UserName.ToLower().Trim())
                || (u.FirstName + " " + u.LastName).ToLower().Contains(filter.UserName.ToLower()));

            var result = await (from u in users
                select new GetUserProfileDto()
                {
                    UserName = u.ApplicationUser.UserName,
                    UserId = u.ApplicationUserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Dob = u.Dob,
                    Image = u.Image,
                    UserRoles = (from r in context.Roles
                        join ur in context.UserRoles on r.Id equals ur.RoleId
                        where ur.UserId == u.ApplicationUserId
                        select new GetUserRoleDto()
                        {
                            Id = r.Id, Name = r.Name
                        }).ToList()
                })
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var totalRecord = await users.CountAsync();

            return new PagedResponse<List<GetUserProfileDto>>(result, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetUserProfileDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetUserProfileDto>> GetUserProfileById([Required] string id)
    {
        try
        {
            var user = await context.UserProfiles
                .Select(u => new GetUserProfileDto
                {
                    UserName = u.ApplicationUser.UserName!,
                    UserId = u.ApplicationUserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Dob = u.Dob,
                    Image = u.Image,
                    UserRoles = (from r in context.Roles
                        join ur in context.UserRoles on r.Id equals ur.RoleId
                        where ur.UserId == u.ApplicationUserId
                        select new GetUserRoleDto()
                        {
                            Id = r.Id, Name = r.Name
                        }).ToList()
                })
                 .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user != null)
                return new Response<GetUserProfileDto>(user);

            return new Response<GetUserProfileDto>(HttpStatusCode.NotFound, "User not found!");
        }
        catch (Exception e)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> UpdateUserProfileDto(AddUserProfileDto addUserProfile, string userId)
    {
        try
        {
            var user = await context.UserProfiles.FindAsync(userId);
            if (user == null)
                return new Response<bool>(HttpStatusCode.NotFound, "Current user not found");

            user.FirstName = addUserProfile.FirstName;
            user.LastName = addUserProfile.LastName;
            user.Email = addUserProfile.Email;
            user.PhoneNumber = addUserProfile.PhoneNumber;
            user.Dob = addUserProfile.Dob;

            fileService.DeleteFile(user.Image);

            var image = await fileService.CreateFile(addUserProfile.Image!);

            user.Image = image.Data!;

            await context.SaveChangesAsync();

            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> DeleteUser([Required] string id)
    {
        try
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return new Response<string>(HttpStatusCode.NotFound, "User not found!");

            await userManager.DeleteAsync(user);

            return new Response<string>("The user was successfully deleted.");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> AddRoleFromUser(AddRoleFromUserDto addRoleFromUser)
    {
        try
        {
            var user = await context.Users.FindAsync(addRoleFromUser.UserId);
            if (user == null)
                return new Response<string>(HttpStatusCode.NotFound, "User not found!");

            var role = await context.Roles.FindAsync(addRoleFromUser.RoleId);
            if (role == null) 
                return new Response<string>(HttpStatusCode.NotFound, "Role not found!");

            var userRole = await userManager.IsInRoleAsync(user, role.Name!);
            if (userRole)
            {
                return new Response<string>(HttpStatusCode.Conflict, "This user already has this role.");
            }

            await userManager.AddToRoleAsync(user, role.Name!);

            return new Response<string>("Roll added successfully.");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> RemoveRoleFromUser(RemoveRoleFromUserDto roleFromUser)
    {
        try
        {
            var user = await context.Users.FindAsync(roleFromUser.UserId);
            if (user == null)
                return new Response<string>(HttpStatusCode.NotFound, "User not found!");

            var role = await context.Roles.FindAsync(roleFromUser.RoleId);
            if (role == null) 
                return new Response<string>(HttpStatusCode.NotFound, "Role not found!");

            var userRole = await userManager.IsInRoleAsync(user, role.Name!);
            if (!userRole)
                return new Response<string>(HttpStatusCode.NotFound, "This user does not have this role.");

            await userManager.RemoveFromRoleAsync(user, role.Name!);

            return new Response<string>("Role deleted successfully.");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<List<GetUserRoleDto>>> GetUserRoles()
    {
        try
        {
            var roles = await context.Roles
                .Where(x => x.Name != "SuperAdmin")
                .Select(r => new GetUserRoleDto()
                {
                    Id = r.Id,
                    Name = r.Name!
                })
                .AsNoTracking()
                .ToListAsync();

            return new Response<List<GetUserRoleDto>>(roles);
        }
        catch (Exception e)
        {
            return new Response<List<GetUserRoleDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}