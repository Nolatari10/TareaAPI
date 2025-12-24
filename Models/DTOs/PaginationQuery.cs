using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TareaAPI.Models.DTOs
{
    public class PaginationQuery
    {
        private const int MaxPageSize = 100;
        public int Page { get; set; } = 1;
        private int _size = 10;
        public int PageSize
        {
            get => _size;
            set => _size = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}