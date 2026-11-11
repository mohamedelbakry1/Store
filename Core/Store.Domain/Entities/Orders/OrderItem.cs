namespace Store.Domain.Entities.Orders
{
    public class OrderItem : BaseEntity<int>
    {
        public ProductInOrderItem Product { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; } = default!;
        public Guid OrderId { get; set; }
    }
}