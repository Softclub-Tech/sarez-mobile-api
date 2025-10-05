using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Seed;
using Infrastructure.Services.AccountService;
using Infrastructure.Services.BrandService;
using Infrastructure.Services.CartService;
using Infrastructure.Services.CategoryService;
using Infrastructure.Services.ColorService;
using Infrastructure.Services.FileService;
using Infrastructure.Services.ProductService;
using Infrastructure.Services.SmartphoneService;
using Infrastructure.Services.SubCategoryService;
using Infrastructure.Services.TelevisionService;
using Infrastructure.Services.UserProfileService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi.ExtensionMethods.RegisterService;

public static class RegisterService
{
    public static void AddRegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(configure =>
            configure.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IColorService, ColorService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISmartphoneService, SmartphoneService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<ITelevisionService, TelevisionService>();
        services.AddScoped<Seeder>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<ICartService, CartService>();
        
        services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false; // must have at least one digit
                config.Password.RequireNonAlphanumeric = false; // must have at least one non-alphanumeric character
                config.Password.RequireUppercase = false; // must have at least one uppercase character
                config.Password.RequireLowercase = false;  // must have at least one lowercase character
            })
            //for registering usermanager and signinmanger
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();
    }
}