namespace _1likte_task.Entities;

public class Cart
{
    public int CartId { get; set; }
    public int UserId { get; set; }
    public List<CartItem> Items { get; set; } = null!;

    public decimal TotalPrice { get; set; }
    public void UpdateTotalPrice()
    {
        TotalPrice = Items.Sum(item => item.Price * item.Quantity);
    }
}