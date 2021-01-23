namespace WebStore.Domain.DTOs.Products
{
    public record SectionDto(
        int Id, 
        string Name, 
        int Order, 
        int? ParentId,
        int ProductsCount);
}
