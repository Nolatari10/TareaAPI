using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TareaAPI.Models;
using TareaAPI.Models.DTOs;

namespace TareaAPI.Repositories
{
    public interface ICategoriaRepository
    {
        Task<PaginatedResponse<Categoria>> GetAllCategoriasAsync(PaginationQuery query);
        Task<Categoria?> GetCategoriaByIdAsync(int id);
        Task<Categoria> CreateCategoriaAsync(Categoria categoria);
        Task UpdateCategoriaAsync(Categoria categoria);
        Task DeleteCategoriaAsync(Categoria categoria);
    }
}