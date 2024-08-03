using _1likte_task.Entities;
using Microsoft.EntityFrameworkCore;

namespace _1likte_task.Repositories;

public class CartRepository : ICartRepository
{
    private readonly Context _context;

    public CartRepository(Context context)
    {
        _context = context;
    }

    public async Task<Cart?> GetCart(int userId)
    {
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart != null) return cart;
        cart = new Cart { UserId = userId };
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task AddItem(int userId, CartItem item)
    {
        var cart = await GetCart(userId);

        var existingItem =
            cart?.Items.FirstOrDefault(i => i.ProductId == item.ProductId && i.ProductStoreId == item.ProductStoreId);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            cart?.Items.Add(item);
        }

        cart?.UpdateTotalPrice();
        await _context.SaveChangesAsync();
    }

    public async Task RemoveItem(int userId, int productId, Guid productStoreId)
    {
        var cart = await GetCart(userId);
        var item = cart?.Items.FirstOrDefault(i => i.ProductId == productId && i.ProductStoreId == productStoreId);
        if (item != null)
        {
            cart?.Items.Remove(item);
            cart?.UpdateTotalPrice();
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateCart(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task ClearCart(int userId)
    {
        var cart = await GetCart(userId);
        if (cart != null)
        {
            _context.Carts.Remove(cart);
            cart.TotalPrice = 0;
            await _context.SaveChangesAsync();
        }
    }
}