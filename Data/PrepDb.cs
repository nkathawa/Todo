using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo.Enums;
using Todo.Models;

namespace Todo.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), userManager, roleManager, isProduction);
            }
        }

        private static void SeedData(AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            bool isProduction)
        {
            SeedTodoItems(context, isProduction);
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedTodoItems(AppDbContext context, bool isProduction)
        {
            if (isProduction)
            {
                Console.WriteLine("--> applying migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if (!context.TodoItems.Any())
            {
                Console.WriteLine("--> Seeding data...");

                context.TodoItems.AddRange(
                    new TodoItem() { Id = 1, Text = "text Mom", UserId = 1, Status = StatusType.OPEN },
                    new TodoItem() { Id = 2, Text = "get groceries", UserId = 1, Status = StatusType.OPEN },
                    new TodoItem() { Id = 3, Text = "clean room", UserId = 1, Status = StatusType.CLOSED }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have todo data");
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole("Admin");
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole("User");
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                    FirstName = "Bob",
                    LastName = "Smith"
                };

                IdentityResult result = userManager.CreateAsync(user, "AdminPassword123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            if (userManager.FindByNameAsync("user").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "user",
                    Email = "user@example.com",
                    EmailConfirmed = true,
                    FirstName = "Ron",
                    LastName = "Johnson"
                };

                IdentityResult result = userManager.CreateAsync(user, "UserPassword123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }
        }
    }
}