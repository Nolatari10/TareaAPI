using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TareaAPI.Models.DTOs
{
    public class CategoriaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; }  = string.Empty;

    }
    public class CategoriaCreateDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; }  = string.Empty;
    }

    public class CategoriaUpdateDto
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; }  = string.Empty;
    }
}