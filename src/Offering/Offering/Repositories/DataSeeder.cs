using Offering.Models;

namespace Offering.Repositories;

public class DataSeeder
{
    public static async Task SeedData(DataContext context)
    {
        if (context.Products.ToList().Count == 0)
        {
            var products = new List<Product>
            {
                Product.Create(Guid.Parse("fd239078-28ff-4158-a79c-ad533ce26dc5"), "Product 1", "Product 1 description"),
                Product.Create(Guid.Parse("b7c196c6-e082-460f-af7f-0262f960d2ee"), "Product 2", "Product 2 description"),
                Product.Create(Guid.Parse("a7948abe-571c-4e9d-a7f3-94a88ed10125"), "Product 2", "Product 2 description")
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }

        if (context.Offers.ToList().Count == 0)
        {
            var price1 = Price.CreateFromGross(119, 19, "EUR");
            var products = context.Products.ToList();
            var price2 = Price.CreateFromGross(99, 19, "EUR");
            var offers = new List<Offer>
            {
                SingleOffer.Create("Offer 1", price1, DateTime.Now, DateTime.Now, products.First()),
                PackageOffer.Create("Offer 2", price2, DateTime.Now, DateTime.Now, products)
            };
            context.Offers.AddRange(offers);
            await context.SaveChangesAsync();
        }
    }
}