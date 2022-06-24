using BookStore.DataBase;
using BookStore.DataBaseEntities;
using Microsoft.EntityFrameworkCore;

namespace BookStore;

public static class DatabaseSeed
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<BookStoreDbContext>());
        }

        void SeedData(BookStoreDbContext context)
        {
            System.Console.WriteLine("Appling migrations..");
            context.Database.Migrate();
            if (!context.Roles.Any())
            {
                System.Console.WriteLine("Adding data - seeding...");
                context.Roles.AddRange(
                    new Role()
                    {
                        Name = "User"
                    },
                    new Role()
                    {
                        Name = "Admin"
                    }
                );
                context.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("Already have data - not seeding");
            }
        }
    }
}