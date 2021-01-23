namespace WebStore.Domain.DTOs.Orders
{
    public record OrderItemDto(
        int Id, 
        decimal Price,
        int Quantity);
}
