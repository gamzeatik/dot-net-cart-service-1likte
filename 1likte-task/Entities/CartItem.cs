namespace _1likte_task.Entities;

public class CartItem
{
    public int CartItemId { get; set; }
    public Guid ProductStoreId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}