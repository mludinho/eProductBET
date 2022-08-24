using eProducts.Data.Static;
using eProducts.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProducts.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                //Products
                if (!context.Products.Any())
                {
                    context.Products.AddRange(new List<Product>()
                    {
                        new Product()
                        {
                            Name = "Adidas",
                            Description = "This is Addias shoe",
                            Price = 599.50,
                            ImageURL = "/images/products/adidas_shoe-3.png",
                            CreatedDate = DateTime.Now.AddDays(3),
                            ProductCategory = ProductCategory.Sports
                        },
                        new Product()
                        {
                            Name = "Soccer Ball",
                            Description = "Branded soccer balls",
                            Price = 399.50,
                            ImageURL = "/images/products/adidas_football-1.png",
                            CreatedDate = DateTime.Now.AddDays(3),
                            ProductCategory = ProductCategory.Sports
                        },
                        new Product()
                        {
                            Name = "Racket",
                            Description = "Branded rackets",
                            Price = 650.00,
                            ImageURL = "/images/products/victor-racket-2.png",
                            CreatedDate = DateTime.Now.AddDays(3),
                            ProductCategory = ProductCategory.Sports
                        },
                        new Product()
                        {
                            Name = "Puma",
                            Description = "Puma shoes",
                            Price = 950.70,
                            ImageURL = "/images/products/puma_shoe-1.png",
                            CreatedDate = DateTime.Now.AddDays(3),
                            ProductCategory = ProductCategory.Sports
                        },
                        new Product()
                        {
                            Name = "Kit back",
                            Description = "Yonex kit back",
                            Price = 789.50,
                            ImageURL = "/images/products/babolat-kitback-1.png",
                            CreatedDate = DateTime.Now.AddDays(3),
                            ProductCategory = ProductCategory.Sports
                        },
                        new Product()
                        {
                            Name = "Asics",
                            Description = "Asics brands",
                            Price = 569.00,
                            ImageURL = "/images/products/asics_shoe-3.png",
                            CreatedDate = DateTime.Now.AddDays(3),
                            ProductCategory = ProductCategory.Sports
                        }
                    }) ;
                    context.SaveChanges();
                }
            }

        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "admin@eproducts.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if(adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Product@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }


                string appUserEmail = "user@eproducts.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Application User",
                        UserName = "user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "Product@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
