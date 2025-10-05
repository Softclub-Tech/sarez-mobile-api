using Domain.Dtos.UserProfileDtos;
using Domain.Dtos.UserRoleDtos;
using Domain.Filters;
using Domain.Response;

namespace Infrastructure.Services.UserProfileService;

public interface IUserProfileService
{
    Task<PagedResponse<List<GetUserProfileDto>>> GetUserProfiles(UserProfileFilter filter);
    Task<Response<GetUserProfileDto>> GetUserProfileById(string id);
    Task<Response<bool>> UpdateUserProfileDto(AddUserProfileDto addUserProfile, string userId);
    Task<Response<string>> DeleteUser(string id);
    Task<Response<string>> AddRoleFromUser(AddRoleFromUserDto roleFromUserDto);
    Task<Response<string>> RemoveRoleFromUser(RemoveRoleFromUserDto roleFromUser);
    Task<Response<List<GetUserRoleDto>>> GetUserRoles();
}