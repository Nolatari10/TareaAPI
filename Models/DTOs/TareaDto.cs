using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TareaAPI.Models.DTOs
{
    public class TareaDto
    {
        public string Nombre { get; set; } = string.Empty;
    public bool Completada { get; set; }
    }

    public class TareaCreateDto
{
    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Nombre { get; set; } = string.Empty;

    public bool Completada {get; set; }

    //ADDED CategoryId to DTO
    public int? CategoriaId { get; set; } //nullable
}

public class TareaUpdateDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Nombre { get; set; } = string.Empty;

    public bool Completada { get; set; }
    //ADDED CategoryId to DTO
    public int? CategoriaId { get; set; } //nullable
}
}
