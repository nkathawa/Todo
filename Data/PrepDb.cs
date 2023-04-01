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
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                if (dbContext != null)
                {
                    SeedData(dbContext, userManager, roleManager, isProduction);
                }
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

            var todoItems = context.TodoItems;
            if (todoItems != null && !todoItems.Any())
            {
                Console.WriteLine("--> Seeding data...");

                todoItems.AddRange(
                    new TodoItem() { Id = 1, Title = "text Mom", Description = "foo", UserId = "abc123", Status = StatusType.OPEN },
                    new TodoItem() { Id = 2, Title = "get groceries", Description = "foo", UserId = "abc123", Status = StatusType.OPEN },
                    new TodoItem() { Id = 3, Title = "clean room", Description = "foo", UserId = "abc123", Status = StatusType.ARCHIVED }
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
                    LastName = "Smith",
                    Id = Guid.NewGuid().ToString()
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
                    LastName = "Johnson",
                    Id = Guid.NewGuid().ToString()
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