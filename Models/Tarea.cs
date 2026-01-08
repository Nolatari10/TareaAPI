using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TareaAPI.Models
{
    public class Tarea
    {
         public int Id { get; set; }
    public string? Nombre { get; set; }
    public bool Completada { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
   
    //ADDED CategoryId for relationship
    public int? CategoriaId { get; set; } //fk nullable
    public Categoria? Categoria { get; set; } //prop navigation
    }
}