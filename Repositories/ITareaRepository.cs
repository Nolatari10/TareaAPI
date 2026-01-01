using TareaAPI.Models;
using TareaAPI.Models.DTOs;
namespace TareaAPI.Repositories
{
    public interface ITareaRepository
    {
        Task<PaginatedResponse<Tarea>> GetAllTareasAsync(PaginationQuery query);
        Task<Tarea?> GetTareaByIdAsync(int id);
        Task<Tarea> CreateTareaAsync(Tarea tarea);
        Task UpdateTareaAsync(Tarea tarea);
        Task DeleteTareaAsync(Tarea tarea);
    }
}