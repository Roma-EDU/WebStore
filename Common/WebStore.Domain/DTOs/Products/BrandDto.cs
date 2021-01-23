namespace WebStore.Domain.DTOs.Products
{
    public record BrandDto(
        int Id, 
        string Name, 
        int Order,
        int ProductsCount);
}
