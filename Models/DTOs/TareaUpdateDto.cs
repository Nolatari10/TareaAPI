using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TareaAPI.Models.DTOs
{
    public class TareaUpdateDto
    {
        public string? Nombre { get; set; }
        public bool Completada { get; set; }
    }
}