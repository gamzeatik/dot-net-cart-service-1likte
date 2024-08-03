using _1likte_task.Entities;
using _1likte_task.Repositories;

namespace _1likte_task.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository, Context context)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Cart?> GetCartAsync(int userId)
    {
        return await _cartRepository.GetCart(userId);
    }

    public async Task AddItemAsync(int userId, CartItem item)
    {
        await _cartRepository.AddItem(userId, item);
    }

    public async Task RemoveItemAsync(int userId, int productId, Guid productStoreId)
    {
        await _cartRepository.RemoveItem(userId, productId, productStoreId);
    }


    public async Task ClearCartAsync(int userId)
    {
        await _cartRepository.ClearCart(userId);
    }
}