using Microsoft.EntityFrameworkCore;
using Todo.Enums;
using Todo.Models;

namespace Todo.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
            }
        }

        private static void SeedData(AppDbContext context, bool isProduction)
        {
            if(isProduction)
            {
                Console.WriteLine("--> applying migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if(!context.TodoItems.Any())
            {
                Console.WriteLine("--> Seeding data...");

                context.TodoItems.AddRange(
                    new TodoItem() {Id=1, Text="text Mom", UserId=1, Status=StatusType.OPEN},
                    new TodoItem() {Id=2, Text="get groceries", UserId=1, Status=StatusType.OPEN},
                    new TodoItem() {Id=3, Text="clean room", UserId=1, Status=StatusType.CLOSED}
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}