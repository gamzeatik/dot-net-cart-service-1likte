using _1likte_task.Entities;

namespace _1likte_task.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetCart(int userId);
    Task AddItem(int userId, CartItem item);
    Task RemoveItem(int userId, int productId, Guid productStoreId);
    Task UpdateCart(Cart cart);
    Task ClearCart(int userId);
}