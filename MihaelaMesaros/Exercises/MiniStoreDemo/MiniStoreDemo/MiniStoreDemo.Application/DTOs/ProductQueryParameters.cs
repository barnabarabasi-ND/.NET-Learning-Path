using System.ComponentModel.DataAnnotations;

namespace MiniStoreDemo.Application.DTOs
{
    public class ProductQueryParameters
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        public int? CategoryId { get; set; }

        public bool? IsActive { get; set; }

        public string? Keyword { get; set; }
    }
}
