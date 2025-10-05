using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed;

public class Seeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
    ApplicationContext context)
{
    public async Task SeedRole()
    {
        var newRoles = new List<IdentityRole>()
        {
            new(DefaultRoleConst.SuperAdmin),
            new(DefaultRoleConst.Admin),
            new(DefaultRoleConst.User)
        };
        var existing = await roleManager.Roles.ToListAsync();
        foreach (var role in newRoles)
        {
            if (!existing.Exists(r => r.Name == role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    public async Task SeedAdmin()
    {
        var existing = await userManager.FindByNameAsync("admin");
        if (existing != null) return;
        var identity = new ApplicationUser()
        {
            UserName = "admin",
            PhoneNumber = "+992005442641",
            Email = "shinoyatzoda@gmail.com"
        };
        await userManager.CreateAsync(identity, "hello123");
        await userManager.AddToRoleAsync(identity, DefaultRoleConst.Admin);
        var userProfile = new UserProfile()
        {
            ApplicationUserId = identity.Id,
            FirstName = "",
            LastName = "",
            Email = "",
            PhoneNumber = "",
            Image = ""
        };
        await context.UserProfiles.AddAsync(userProfile);
        await context.SaveChangesAsync();
    }
    
    public async Task SeedSuperAdmin()
    {
        var existing = await userManager.FindByNameAsync("SuperAdmin");
        if (existing != null) return;
        var identity = new ApplicationUser()
        {
            UserName = "SuperAdmin",
            PhoneNumber = "+992005442641",
            Email = "shinoyatzoda@gmail.com"
        };
        await userManager.CreateAsync(identity, "SuperAdmin2024");
        await userManager.AddToRoleAsync(identity, DefaultRoleConst.SuperAdmin);
        var userProfile = new UserProfile()
        {
            ApplicationUserId = identity.Id,
            FirstName = "Shodmon",
            LastName = "Inoyatzoda",
            Email = "shinoyatzoda@gmail.com",
            PhoneNumber = "+992005442641",
            Image = "OIP.jpg"
        };
        await context.UserProfiles.AddAsync(userProfile);
        await context.SaveChangesAsync();
    }

    public async Task SeedCatalog()
    {
        var i = 0;
        foreach (var catalog in new SeedCatalog().Catalogs)
        {
            var newCategory = new Category()
            {
                CategoryName = catalog,
                CategoryImage = ""
            };
            var exist = await context.Categories.AnyAsync(c => c.CategoryName == catalog);
            if (exist)
            {
                i++;
                continue;
            }
            await context.Categories.AddAsync(newCategory);
            await context.SaveChangesAsync();
            foreach (var category in new SeedCatalog().Category[i])
            {
                var newSubCategory = new SubCategory()
                {
                    CategoryId = newCategory.Id,
                    SubCategoryName = category
                };
                await context.SubCategories.AddAsync(newSubCategory);
                await context.SaveChangesAsync();
            }

            i++;
        }
    }
}
