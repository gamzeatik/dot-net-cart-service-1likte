using _1likte_task.Entities;

namespace _1likte_task.Services;

public interface ICartService
{
    Task<Cart?> GetCartAsync(int userId);
    Task AddItemAsync(int userId, CartItem cartItem);
    Task RemoveItemAsync(int userId, int productId, Guid productStoreId);
    Task ClearCartAsync(int userId);
}