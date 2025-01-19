using System;
using EShop.Entity.Abstract;

namespace EShop.Entity.Concrete;

public class CartItem : BaseEntity
{
    private CartItem()
    {
    }
    public CartItem(int cartId, int productId, int quantity) //sadece veritanbanındaki karşılığı olan class
    {
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
    }

    public int CartId { get; set; }
    public Cart? Cart { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
}
