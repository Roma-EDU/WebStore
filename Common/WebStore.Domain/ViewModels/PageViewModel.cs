using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.ViewModels
{
    /// <summary> Модель постраничного разбития </summary>
    public class PageViewModel
    {
        /// <summary> Общее количество элементов </summary>
        public int TotalItems { get; set; }

        /// <summary> Количество элементов на одной странице </summary>
        public int PageSize { get; set; }

        /// <summary> Текущая страница </summary>
        public int PageNumber { get; set; }

        /// <summary> Общее количество страниц </summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((decimal)TotalItems / PageSize) : 1;
    }

}
