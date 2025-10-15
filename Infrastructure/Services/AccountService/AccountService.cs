using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.Dtos.AccountDtos;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.AccountService;

public class AccountService(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    ApplicationContext context) : IAccountService
{
    public async Task<Response<string>> Register(RegisterDto model)
    {
        try
        {
            var user = new ApplicationUser()
            {
                UserName = model.UserName.ToLower().Trim(),
                Email = model.Email.ToLower().Trim(),
                PhoneNumber = model.PhoneNumber.Trim()
            };

            await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRoleAsync(user, DefaultRoleConst.User);

            var userProfile = new UserProfile()
            {
                ApplicationUserId = user.Id,
                FirstName = "",
                LastName = "",
                Email = model.Email.ToLower().Trim(),
                PhoneNumber = model.PhoneNumber.Trim(),
                Image = ""
            };

            await context.UserProfiles.AddAsync(userProfile);
            await context.SaveChangesAsync();

            return new Response<string>("Registered successfully.");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> Login(LoginDto model)
    {
        try
        {
            var user = await userManager.FindByNameAsync(model.UserName.ToLower().Trim());
            if (user == null)
                return new Response<string>(HttpStatusCode.BadRequest, "Your UserName or Password is incorrect!!!");

            var check = await userManager.CheckPasswordAsync(user, model.Password);
            return check
                ? new Response<string>(await GenerateJwtToken(user))
                : new Response<string>(HttpStatusCode.BadRequest, "Your UserName or Password is incorrect!!!");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    private async Task<string> GenerateJwtToken(ApplicationUser applicationUser)
    {
        var userProfile = await context.UserProfiles.FindAsync(applicationUser.Id);

        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sid, applicationUser.Id),
            new(JwtRegisteredClaimNames.Name, applicationUser.UserName!),
            new(JwtRegisteredClaimNames.Email, applicationUser.Email!),
            new(JwtRegisteredClaimNames.Sub, userProfile!.Image)
        };
        //add roles
        var roles = await userManager.GetRolesAsync(applicationUser);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(10),
            signingCredentials: credentials
        );

        var securityTokenHandler = new JwtSecurityTokenHandler();
        var tokenString = securityTokenHandler.WriteToken(token);
        return tokenString;
    }
}