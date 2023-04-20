using AutoMapper;
using Inventory.Application.Exceptions;
using ShoppingCart.Application.Dtos;
using ShoppingCart.Core.Product;
using ShoppingCart.Core.ShoppingCart;

namespace ShoppingCart.Application.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingBasketRepository;
    
    private readonly IProductRepository _productRepository;
    
    private readonly IMapper _mapper;
    
    public ShoppingCartService(IShoppingCartRepository shoppingBasketRepository, IProductRepository productRepository, IMapper mapper)
    {
        _shoppingBasketRepository = shoppingBasketRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task AddProductToShoppingCart(Guid customerId, Guid productId, int quantity)
    {
        var shoppingCart = await GetActiveShoppingCartFromRepository(customerId);
        var product = await GetProductFromRepository(productId);
        if (shoppingCart == null)
        {
            // when the user currently has no active shopping cart, one will be created
            shoppingCart = ShoppingCart.Core.ShoppingCart.ShoppingCart.Create(customerId);
            await _shoppingBasketRepository.Create(shoppingCart);
        }
        shoppingCart.AddItem(product, quantity);
        await _shoppingBasketRepository.Update(shoppingCart);
    }

    public async Task RemoveProductFromShoppingCart(Guid customerId, Guid productId)
    {
        var shoppingCart = await GetActiveShoppingCartFromRepository(customerId);
        if (shoppingCart == null)
        {
            throw new NoActiveShoppingBasketFoundException("No active shopping cart found for the customer.");
        }
        var product = await GetProductFromRepository(productId);
        shoppingCart.RemoveItem(product);
        await _shoppingBasketRepository.Update(shoppingCart);
    }

    public async Task<ShoppingCartResponseDto> GetActiveShoppingCart(Guid customerId)
    {
        var shoppingCart = await GetActiveShoppingCartFromRepository(customerId);
        return _mapper.Map<Core.ShoppingCart.ShoppingCart, ShoppingCartResponseDto>(shoppingCart);
    }
    
    private async Task<ShoppingCart.Core.ShoppingCart.ShoppingCart> GetActiveShoppingCartFromRepository(Guid customerId)
    {
        var shoppingCart = await _shoppingBasketRepository.GetActiveShoppingCartByCustomer(customerId);
        return shoppingCart;
    }

    public async Task Checkout(Guid customerId, CheckoutRequestDto checkoutRequestDto)
    {
        var shoppingCart = await GetActiveShoppingCartFromRepository(customerId);
        var address = new Address(checkoutRequestDto.ShippingAddress.Street, checkoutRequestDto.ShippingAddress.City, checkoutRequestDto.ShippingAddress.ZipCode, checkoutRequestDto.ShippingAddress.Country);
        Address billingAddress = null;
        if (checkoutRequestDto.BillingAddress != null)
        {
            billingAddress = new Address(checkoutRequestDto.BillingAddress.Street, checkoutRequestDto.BillingAddress.City, checkoutRequestDto.BillingAddress.ZipCode, checkoutRequestDto.BillingAddress.Country);
        }
        var checkout = new ShoppingCartCheckout(checkoutRequestDto.CustomerId, checkoutRequestDto.FirstName, checkoutRequestDto.LastName, checkoutRequestDto.Email, address, billingAddress);
        
        shoppingCart.Checkout(checkout);
        _shoppingBasketRepository.Update(shoppingCart);
    }
    
    private async Task<Product> GetProductFromRepository(Guid productId)
    {
        var product = await _productRepository.GetById(productId);
        if (product == null)
        {
            throw new EntityNotFoundException("Product not found.");
        }
        return product;
    }

    public async Task TimeOutShoppingCarts()
    {
        var shoppingCarts = await _shoppingBasketRepository.GetActiveShoppingCartsCreatedBefore(DateTime.Now.AddMinutes(-30));
        foreach (var shoppingCart in shoppingCarts)
        {
            shoppingCart.MarkAsTimedOut();
            await _shoppingBasketRepository.Update(shoppingCart);
        }
    }
}