using Quartz;

namespace ShoppingCart.Application.Services;

public class TimeOutShoppingCartsJob : IJob
{
    private readonly IShoppingCartService _shoppingCartService;
    
    public TimeOutShoppingCartsJob(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }
    
    public Task Execute(IJobExecutionContext context)
    {
        return _shoppingCartService.TimeOutShoppingCarts();
    }
}