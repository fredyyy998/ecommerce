using ShoppingCart.Core.Events;
using ShoppingCart.Core.Exceptions;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Test.ProductTest;

public class ProductTest
{
    [Fact]
    public void Product_Stock_ShouldNotBeSmallerThanZero_WhenCreating()
    {
        var id = Guid.NewGuid();
        var name = "Test";
        var description = "Test";
        var price = new Price(10, 10, "EUR");
        var stock = -1;

        Assert.Throws<ProductDomainException>(() => Product.Create(id, name, description, price, stock));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Product_Stock_ShouldBeGreaterThanZero_WhenUpdating(int updatedStock)
    {
        var name = "Test";
        var description = "Test";
        var price = new Price(10, 10, "EUR");

        var product = CreateProduct();

        Assert.Throws<ProductDomainException>(() => product.Update(name, description, price, updatedStock));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Product_Stock_ShouldBeGreaterThanZero_WhenRemovingStock(int removeQuantity)
    {
        var product = CreateProduct();

        Assert.Throws<ProductDomainException>(() => product.RemoveStock(removeQuantity));
    }
    
    [Fact]
    public void New_Product_Stock_ShouldNotBeSmallerThanZero_AfterRemovingStock()
    {
        var product = CreateProduct();
        
        Assert.Throws<ProductDomainException>(() => product.RemoveStock(2));
    }
    
    [Fact]
    public void Product_Stock_Input_ShouldBeGreaterThanZero_WhenAddingStock()
    {
        var product = CreateProduct();
        
        Assert.Throws<ProductDomainException>(() => product.AddStock(0));
    }
    
    [Fact]
    public void Product_Can_Be_Reserved_When_Stock_Is_Greater_Than_Reservation()
    {
        var product = CreateProduct();
        var shoppingCartId = Guid.NewGuid();
        
        product.Reservate(1, shoppingCartId);
        
        Assert.Equal(product.TotalReserved, 1);
        Assert.Equal(product.Reservations.Count, 1);
    }
    
    [Fact]
    public void Product_Can_Not_Be_Reserved_When_Stock_Is_Smaller_Than_Reservation()
    {
        var product = CreateProduct();
        var shoppingCartId = Guid.NewGuid();
        
        Assert.Throws<ProductDomainException>(() => product.Reservate(2, shoppingCartId));
    }
    
    [Fact]
    public void Product_Can_Not_Be_Reserved_When_TotalReservations_Is_Smaller_than_Zero()
    {
        var product = CreateProduct(5);
        var shoppingCartId = Guid.NewGuid();
        product.Reservate(5, shoppingCartId);
        
        Assert.Throws<ProductDomainException>(() => product.Reservate(2, shoppingCartId));
    }

    [Fact]
    public void Product_Reservations_will_be_canceled_when_updated_stock_is_insufficient()
    {
        var product = CreateProduct(5);
        var shoppingCartId = Guid.NewGuid();
        product.Reservate(2, shoppingCartId);
        product.Reservate(3, Guid.NewGuid());
        
        product.Update("Test", "Test", new Price(10, 10, "EUR"), 2);
        
        Assert.Equal(product.TotalReserved, 2);
        Assert.Equal(product.Reservations.Count, 1);
    }

    [Fact]
    public void Dumped_Reservation_Event_Will_Be_Created_For_Each_Dumped_Reservation()
    {
        var product = CreateProduct(5);
        var shoppingCartId = Guid.NewGuid();
        var shoppingCartId2 = Guid.NewGuid();
        var shoppingCartId3 = Guid.NewGuid();
        product.Reservate(2, shoppingCartId);
        product.Reservate(1, shoppingCartId2);
        product.Reservate(1, shoppingCartId3);
        product.ClearEvents();
        
        product.Update("Test", "Test", new Price(10, 10, "EUR"), 2);
        
        Assert.Equal(2, product.DomainEvents.Count);
        Assert.IsType<ReservationCanceledDueToStockUpdateEvent>(product.DomainEvents.First());
        Assert.IsType<ReservationCanceledDueToStockUpdateEvent>(product.DomainEvents.Last());
        Assert.Equal(1, ((ReservationCanceledDueToStockUpdateEvent)product.DomainEvents.First()).Quantity);
        Assert.Equal(shoppingCartId2, ((ReservationCanceledDueToStockUpdateEvent)product.DomainEvents.First()).ShoppingCartId);
        Assert.Equal(product.Id, ((ReservationCanceledDueToStockUpdateEvent)product.DomainEvents.First()).ProductId);
    }

    [Fact]
    public void Multiple_Product_Reservation_In_Same_ShoppingCart_Is_Merger()
    {
        var product = CreateProduct(5);
        var shoppingCartId = Guid.NewGuid();
        product.Reservate(2, shoppingCartId);
        product.Reservate(3, shoppingCartId);
        
        Assert.Equal(1, product.Reservations.Count);
        Assert.Equal(3, product.TotalReserved);
    }
    
    [Fact]
    public void Product_Reservations_Will_Be_Removed_When_Canceled()
    {
        var product = CreateProduct(5);
        var shoppingCartId = Guid.NewGuid();
        product.Reservate(2, shoppingCartId);
        
        product.CancelReservation(shoppingCartId);
        
        Assert.Equal(0, product.Reservations.Count);
        Assert.Equal(0, product.TotalReserved);
    }
    
    [Fact]
    public void CommitReservation_Will_Remove_Reservation()
    {
        var product = CreateProduct(5);
        var shoppingCartId = Guid.NewGuid();
        product.Reservate(2, shoppingCartId);
        
        product.CommitReservation(shoppingCartId);
        
        Assert.Equal(0, product.Reservations.Count);
        Assert.Equal(0, product.TotalReserved);
    }

    [Fact]
    public void CommitReservation_Removes_Stock()
    {
        var product = CreateProduct(5);
        var shoppingCartId = Guid.NewGuid();
        product.Reservate(2, shoppingCartId);
        
        product.CommitReservation(shoppingCartId);
        
        Assert.Equal(3, product.Stock);
    }

    private Product CreateProduct(int stock = 1)
    {
        var id = Guid.NewGuid();
        var name = "Test";
        var description = "Test";
        var price = new Price(10, 10, "EUR");

        return Product.Create(id, name, description, price, stock);
    }
}