namespace WebStore.Domain
{
    public class ProductFilter
    {
        /// <summary> Секция, к которой должен принадлежать товар </summary>
        public int? SectionId { get; set; }

        /// <summary> Бренд, к которому должен принадлежать товара </summary>
        public int? BrandId { get; set; }

        /// <summary> Список Id товаров </summary>
        public int[] Ids { get; set; }

        /// <summary> Текущая страница </summary>
        public int Page { get; set; }

        /// <summary> Количество элементов на странице </summary>
        public int? PageSize { get; set; }
    }
}
