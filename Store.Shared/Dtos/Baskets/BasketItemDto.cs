using System.ComponentModel.DataAnnotations;

namespace Store.Shared.DTOS.Baskets
{
    public class BasketItemDTO
    {
        [Required(ErrorMessage = "Product Id is required.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is required.")]
        public string ProductName { get; set; } = default!;
        [Required(ErrorMessage = "Product Picture URL is required.")]
        public string PictureUrl { get; set; } = default!;
        [Required(ErrorMessage = "Product Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Product Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}