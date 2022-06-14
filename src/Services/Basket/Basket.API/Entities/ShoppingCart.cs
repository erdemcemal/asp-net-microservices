namespace Basket.API.Entities;

public class ShoppingCart
{
    public ShoppingCart(string username)
    {
        Username = username;
    }

    public string Username { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();

    public decimal TotalPrice => Items.Sum(item => item.Quantity * item.Price);
}