using Microsoft.EntityFrameworkCore;
using TareaAPI.Data;
using TareaAPI.Models;
using TareaAPI.Models.DTOs;

namespace TareaAPI.Repositories;

public class TareaRepository: ITareaRepository  
{
    private readonly AppDbContext _context;

    public TareaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResponse<Tarea>> GetAllTareasAsync(PaginationQuery query)
    {
        //Logic for pagination that before had in controller
        var totalItems = await _context.Tareas.CountAsync();
        
        var items = await _context.Tareas.Skip((query.Page - 1) * query.PageSize)
                                           .Take(query.PageSize)
                                           .ToListAsync();
        return new PaginatedResponse<Tarea>(items, totalItems, query.Page, query.PageSize);
    }

    public async Task<Tarea?> GetTareaByIdAsync(int id)
    {
        return await _context.Tareas.FindAsync(id);
    }

    public async Task<Tarea> CreateTareaAsync(Tarea tarea)
    {
        _context.Tareas.Add(tarea);
        await  _context.SaveChangesAsync();
        return tarea;
    }
    public async Task UpdateTareaAsync(Tarea tarea)
    {
        _context.Entry(tarea).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTareaAsync(Tarea tarea)
    {
        _context.Tareas.Remove(tarea);
        await _context.SaveChangesAsync();
    }
}