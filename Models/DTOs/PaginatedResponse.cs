using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TareaAPI.Models.DTOs
{
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);

        public bool HasNext => Page < TotalPages;
        public bool HasPrevious => Page > 1;
    }
}