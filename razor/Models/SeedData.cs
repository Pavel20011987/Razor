using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using razor.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;


namespace razor.Models
{
    public static class SeedData
    {   
        public static void InitializeRole(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any Roles.
                if (context.Roles.Any())
                {
                    return;   // DB has been seeded
                }
                context.Roles.AddRange(
                    new IdentityRole{Name = "Admin", NormalizedName = "ADMIN"},
                    new IdentityRole{Name = "User", NormalizedName = "USER"}  
                );     
                context.SaveChanges();                            
            }
        }

        public static void InitializeMask(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any Roles.
                if (context.Masks.Any())
                {
                    return;   // DB has been seeded
                }
                context.Masks.AddRange(
                    new Mask {mask = "16"},
                    new Mask {mask = "17"},                
                    new Mask {mask = "18"},
                    new Mask {mask = "19"},
                    new Mask {mask = "20"},
                    new Mask {mask = "21"},
                    new Mask {mask = "22"},                    
                    new Mask {mask = "23"},   
                    new Mask {mask = "24"},
                    new Mask {mask = "25"},
                    new Mask {mask = "26"},                    
                    new Mask {mask = "27"},               
                    new Mask {mask = "28"},
                    new Mask {mask = "29"},                    
                    new Mask {mask = "30"},   
                    new Mask {mask = "31"},   
                    new Mask {mask = "32"}
                );     
                context.SaveChanges();
            } 
        }
        public static void InitializeVlan(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any Roles.
                if (context.Vlans.Any())
                {
                    return;   // DB has been seeded
                }
                context.Vlans.AddRange(
                    new Vlan {vlan = 1221},
                    new Vlan {vlan = 1222},                
                    new Vlan {vlan = 1223},
                    new Vlan {vlan = 1224},
                    new Vlan {vlan = 1225},
                    new Vlan {vlan = 1226},
                    new Vlan {vlan = 1227},                    
                    new Vlan {vlan = 1228},   
                    new Vlan {vlan = 1229},
                    new Vlan {vlan = 1230},
                    new Vlan {vlan = 1231},                    
                    new Vlan {vlan = 1232},               
                    new Vlan {vlan = 1233},
                    new Vlan {vlan = 1234},                    
                    new Vlan {vlan = 1235},   
                    new Vlan {vlan = 1236},   
                    new Vlan {vlan = 1237}, 
                    new Vlan {vlan = 1238},               
                    new Vlan {vlan = 1239},
                    new Vlan {vlan = 1240},                    
                    new Vlan {vlan = 1241},   
                    new Vlan {vlan = 1242},   
                    new Vlan {vlan = 1243},  
                    new Vlan {vlan = 1244},   
                    new Vlan {vlan = 1245},   
                    new Vlan {vlan = 1246},  
                    new Vlan {vlan = 1247},   
                    new Vlan {vlan = 1248},   
                    new Vlan {vlan = 1249}
                );     
                context.SaveChanges();  
            } 
        }
        public static void InitializeVendor(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any Roles.
                if (context.Vendors.Any())
                {
                    return;   // DB has been seeded
                }
                context.Vendors.AddRange(
                    new Vendor {vendor = "ZTE"},
                    new Vendor {vendor = "HUAWEI"}        
                );     
                context.SaveChanges();   
            } 
        }        
    
    


        public static async Task InitializeUser(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;   // DB has been seeded
                }                
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "Admin@test.com");
                await EnsureRole(serviceProvider, adminID, "Admin");

                // allowed user can create and edit contacts that they create
                var userID = await EnsureUser(serviceProvider, testUserPw, "User@test.com");
                await EnsureRole(serviceProvider, userID, "User");
                context.SaveChanges();

            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                    string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if(user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }
            
            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }   
    
    }
}