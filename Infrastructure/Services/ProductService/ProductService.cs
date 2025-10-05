using System.Net;
using Domain.Dtos.BrandDtos;
using Domain.Dtos.CartDTOs;
using Domain.Dtos.ColorDtos;
using Domain.Dtos.ImageDTOs;
using Domain.Dtos.ProductDtos;
using Domain.Dtos.UserProfileDtos;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ProductService;

public class ProductService(ApplicationContext context, IFileService fileService) : IProductService
{
    public async Task<PagedResponse<GetProductPageDto>> GetProductPage(ProductFilter filter, string? userId)
    {
        try
        {
            var products = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filter.ProductName))
                products = products.Where(p => p.ProductName.ToLower().Contains(filter.ProductName));

            if (!string.IsNullOrEmpty(filter.UserId))
                products = products.Where(p => p.ApplicationUserId == filter.UserId);

            if (filter.MinPrice != 0 && filter.MaxPrice != 0)
                products = products.Where(p => p.Price >= filter.MinPrice && p.Price <= filter.MaxPrice);

            if (filter.BrandId != 0)
                products = products.Where(p => p.BrandId == filter.BrandId);

            if (filter.CategoryId != 0)
                products = products.Where(p => p.SubCategory.CategoryId == filter.CategoryId);

            if (filter.SubcategoryId != 0)
                products = products.Where(p => p.SubCategoryId == filter.SubcategoryId);

            if (filter.ColorId != 0)
                products = products.Where(p => p.ColorId == filter.ColorId);

            var allProducts = await (from p in products
                    select new GetProductsDto()
                    {
                        Id = p.Id,
                        ProductName = p.ProductName,
                        Image = p.ProductImages
                            .Select(i => i.ImageName)
                            .FirstOrDefault()!,
                        Color = p.Color.ColorName,
                        Quantity = p.Quantity,
                        Price = p.Price,
                        CategoryId = p.SubCategory.CategoryId,
                        CategoryName = p.SubCategory.Category.CategoryName,
                        HasDiscount = p.HasDiscountPrice,
                        DiscountPrice = p.DiscountPrice,
                        ProductInMyCart = context.Carts
                            .Any(cart => cart.ProductId == p.Id && cart.ApplicationUserId == userId),
                        ProductInfoFromCart = p.Carts
                            .Where(cart => cart.ProductId == p.Id && cart.ApplicationUserId == userId)
                            .Select(cart =>
                                new CartDto()
                                {
                                    Id = cart.Id,
                                    Quantity = cart.Quantity
                                }).FirstOrDefault()
                    }).OrderByDescending(x => x.Quantity)
                .AsNoTracking()
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            if (allProducts.Count == 0)
                return new PagedResponse<GetProductPageDto>(HttpStatusCode.NoContent, "No product!");

            var rangePrice = new GetMinMaxPriceDto()
            {
                MinPrice = await products.MinAsync(p => p.Price),
                MaxPrice = await products.MaxAsync(p => p.Price)
            };

            var colors = await (from p in products
                select new GetColorDto()
                {
                    Id = p.Color.Id,
                    ColorName = p.Color.ColorName,
                }).Distinct()
                .AsNoTracking()
                .ToListAsync();

            var brands = await (from p in products
                select new GetBrandDto()
                {
                    Id = p.Brand.Id,
                    BrandName = p.Brand.BrandName
                })
                .Distinct()
                .AsNoTracking()
                .ToListAsync();

            var result = new GetProductPageDto()
            {
                Products = allProducts,
                MinMaxPrice = rangePrice,
                Brands = brands,
                Colors = colors
            };

            var totalRecord = await products.CountAsync();

            return new PagedResponse<GetProductPageDto>(result, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<GetProductPageDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetProductDto>> GetProductById(int id, string? userId)
    {
        try
        {
            var product = await (from p in context.Products
                join c in context.Carts on userId equals c.ApplicationUserId into cart
                from c in cart.DefaultIfEmpty()
                select new GetProductDto()
                {
                    Id = p.Id,
                    SubCategoryId = p.SubCategoryId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    BrandId = p.BrandId,
                    Brand = p.Brand.BrandName,
                    ColorId = p.ColorId,
                    Color = p.Color.ColorName,
                    Quantity = p.Quantity,
                    Size = p.Size,
                    Weight = p.Weight,
                    Price = p.Price,
                    HasDiscount = p.HasDiscountPrice,
                    ProductInMyCart = context.Carts
                        .Any(cart => cart.ProductId == p.Id && cart.ApplicationUserId == userId),
                    DiscountPrice = p.DiscountPrice,
                    Code = p.Code,
                    Images = p.ProductImages.Select(i => new GetImageDto()
                    {
                        Id = i.Id,
                        Images = i.ImageName
                    }).ToList(),

                    ProductInfoFromCart = c.ProductId == p.Id
                        ? new CartDto()
                        {
                            Id = c.Id,
                            Quantity = c.Quantity
                        }
                        : new CartDto(),
                    /*GetSmartphone = p.Smartphone != null
                        ? new GetSmartphoneDto()
                        {
                            Id = p.Smartphone.Id,
                            Model = p.Smartphone.Model,
                            Os = p.Smartphone.Os,
                            Communication = p.Smartphone.Communication,
                            Processor = p.Smartphone.Processor,
                            ProcessorFrequency = p.Smartphone.ProcessorFrequency,
                            NumberOfCores = p.Smartphone.NumberOfCores,
                            VideoProcessor = p.Smartphone.VideoProcessor,
                            AspectRatio = p.Smartphone.AspectRatio,
                            DisplayType = p.Smartphone.DisplayType,
                            DisplayResolution = p.Smartphone.DisplayResolution,
                            PixelPerInch = p.Smartphone.PixelPerInch,
                            ScreenRefreshRate = p.Smartphone.ScreenRefreshRate,
                            Diagonal = p.Smartphone.Diagonal,
                            SimCard = p.Smartphone.SimCard,
                            Ram = p.Smartphone.Ram,
                            Rom = p.Smartphone.Rom
                        }
                        : null,*/
                    Users = p.ApplicationUser.Products.Where(u => u.Code == p.Code).Select(pr =>
                        new GetUserShortInfoDto()
                        {
                            UserId = pr.ApplicationUser.Id,
                            UserName = pr.ApplicationUser.UserName!,
                            FullName =
                                pr.ApplicationUser.UserProfile.FirstName + " " +
                                pr.ApplicationUser.UserProfile.FirstName,
                            ImageName = pr.ApplicationUser.UserProfile.Image
                        }).ToList()
                }).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return new Response<GetProductDto>(HttpStatusCode.NotFound, "Product not found");
            return new Response<GetProductDto>(product);
        }
        catch (Exception e)
        {
            return new Response<GetProductDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> AddProduct(AddProductDto addProduct, string userId)
    {
        try
        {
            var existBrand = await context.Brands.AsNoTracking().AnyAsync(b => b.Id == addProduct.BrandId);
            if (!existBrand) return new Response<int>(HttpStatusCode.NotFound, "Brand not found!");
            var existColor = await context.Colors.AsNoTracking().AnyAsync(c => c.Id == addProduct.ColorId);
            if (!existColor) return new Response<int>(HttpStatusCode.BadRequest, "Color not found!");
            var existSubCategory =
                await context.SubCategories.AsNoTracking().AnyAsync(s => s.Id == addProduct.SubCategoryId);
            if (!existSubCategory) return new Response<int>(HttpStatusCode.NotFound, "Sub category not found!");
            if (addProduct is { HasDiscount: true, DiscountPrice: <= 0 })
                return new Response<int>(HttpStatusCode.BadRequest, "The discount price cannot be less then zero!");
            var existProductCode = await context.Products.AsNoTracking().AnyAsync(x => x.Code == addProduct.Code);
            if (existProductCode)
                return new Response<int>(HttpStatusCode.BadRequest,
                    $"This product code: {addProduct.Code} already exist!");
            var product = new Product()
            {
                ApplicationUserId = userId,
                ProductName = addProduct.ProductName,
                Description = addProduct.Description,
                Code = addProduct.Code,
                BrandId = addProduct.BrandId,
                ColorId = addProduct.ColorId,
                Weight = addProduct.Weight,
                Size = addProduct.Size,
                SubCategoryId = addProduct.SubCategoryId,
                Price = addProduct.Price,
                HasDiscountPrice = addProduct.HasDiscount,
                DiscountPrice = addProduct.HasDiscount ? addProduct.DiscountPrice : 0,
                Quantity = addProduct.Quantity
            };

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            foreach (var file in addProduct.Images)
            {
                var imageName = await fileService.CreateFile(file);
                var image = new ProductImage()
                {
                    ProductId = product.Id,
                    ImageName = imageName.Data!
                };
                await context.ProductImages.AddAsync(image);
                await context.SaveChangesAsync();
            }

            return new Response<int>(product.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<int>> UpdateProduct(UpdateProductDto updateProduct, string userId)
    {
        try
        {
            var existBrand = await context.Brands.AsNoTracking().AnyAsync(b => b.Id == updateProduct.BrandId);
            if (!existBrand) return new Response<int>(HttpStatusCode.NotFound, "Brand not found!");
            var existColor = await context.Colors.AsNoTracking().AnyAsync(c => c.Id == updateProduct.ColorId);
            if (!existColor) return new Response<int>(HttpStatusCode.BadRequest, "Color not found!");
            var existSubCategory =
                await context.SubCategories.AsNoTracking().AnyAsync(s => s.Id == updateProduct.SubCategoryId);
            if (!existSubCategory) return new Response<int>(HttpStatusCode.NotFound, "Sub category not found!");
            if (updateProduct is { HasDiscount: true, DiscountPrice: <= 0 })
                return new Response<int>(HttpStatusCode.BadRequest, "The discount price cannot be zero!");
            var existProductCode = await context.Products.AsNoTracking().AnyAsync(x => x.Code == updateProduct.Code);
            if (existProductCode)
                return new Response<int>(HttpStatusCode.BadRequest,
                    $"This product code: {updateProduct.Code} already exist!");
            var product = await context.Products.FindAsync(updateProduct.Id);
            if (product == null) return new Response<int>(HttpStatusCode.NotFound, "Product not found!");
            if (product.ApplicationUserId != userId)
                return new Response<int>(HttpStatusCode.Forbidden, "You do not have access to update this product.");
            product.ProductName = updateProduct.ProductName;
            product.Description = updateProduct.Description;
            product.BrandId = updateProduct.BrandId;
            product.ColorId = updateProduct.ColorId;
            product.Weight = updateProduct.Weight;
            product.Size = updateProduct.Size;
            product.SubCategoryId = updateProduct.SubCategoryId;
            product.Price = updateProduct.Price;
            product.HasDiscountPrice = updateProduct.HasDiscount;
            product.DiscountPrice = updateProduct.HasDiscount ? updateProduct.DiscountPrice : 0;
            product.Quantity = updateProduct.Quantity;
            context.Products.Update(product);
            await context.SaveChangesAsync();
            return new Response<int>(product.Id);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> AddImageToProduct(AddImageToProductDto images)
    {
        try
        {
            var existProduct = await context.Products.Where(product => product.Id == images.ProductId)
                .FirstOrDefaultAsync();
            if (existProduct == null) return new Response<string>(HttpStatusCode.NotFound, "Product not found!");
            var listImages = new List<ProductImage>();
            foreach (var file in images.Files)
            {
                var fileName = await fileService.CreateFile(file);
                var newImage = new ProductImage()
                {
                    ProductId = existProduct.Id,
                    ImageName = fileName.Data!
                };
                listImages.Add(newImage);
            }

            await context.ProductImages.AddRangeAsync(listImages);
            await context.SaveChangesAsync();

            return new Response<string>("Files successfully added.");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> DeleteImageFromProduct(int imageId)
    {
        try
        {
            var existImage = await context.ProductImages.Where(image => image.Id == imageId).AsNoTracking()
                .FirstOrDefaultAsync();
            if (existImage == null)
                return new Response<string>(HttpStatusCode.NotFound, "File not found!");

            fileService.DeleteFile(existImage.ImageName);

            context.ProductImages.Remove(existImage);
            await context.SaveChangesAsync();

            return new Response<string>("File successfully deleted.");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteProduct(int id, string userId)
    {
        try
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
                return new Response<bool>(HttpStatusCode.NotFound, "Product not found!");

            if (product.ApplicationUserId != userId)
                return new Response<bool>(HttpStatusCode.Forbidden, "You do not have access to delete this product.");

            context.Products.Remove(product);
            await context.SaveChangesAsync();

            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}