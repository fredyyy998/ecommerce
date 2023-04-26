using Inventory.Core.Product;

namespace Inventory.Infrastructure;

public class DataSeeder
{
    public static async Task SeedProducts(DataContext context)
    {
        // Skip seeeding if there already is data
        if(context.Products.ToList().Count > 0)
            return;
        var product1 = Product.Create("MacBook", "13-inch, 8GB RAM, 256GB SSD, 2.3GHz Dual-Core Processor", 1299);
        product1.AddStock(10);
        context.Products.Add(product1);

        var product2 = Product.Create("iPhone 12 Pro", "6.1-inch Super Retina XDR display, 5G capable, 128GB storage", 999);
        product2.AddStock(25);
        context.Products.Add(product2);

        var product3 = Product.Create("Samsung Galaxy S21 Ultra", "6.8-inch Dynamic AMOLED 2X, 5G capable, 256GB storage", 1199);
        product3.AddStock(15);
        context.Products.Add(product3);

        var product4 = Product.Create("Sony PlayStation 5", "Gaming console with 4K UHD Blu-ray drive, 825GB SSD storage	", 499);
        product4.AddStock(20);
        context.Products.Add(product4);

        var product5 = Product.Create("LG OLED CX Series 65\" 4K Smart TV	", "65-inch OLED display, 4K UHD, webOS 5.0, Alexa and Google Assistant compatible	", 1999);
        product5.AddStock(5);
        context.Products.Add(product5);
        
        await context.SaveChangesAsync();
    }
}