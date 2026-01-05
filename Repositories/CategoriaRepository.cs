using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TareaAPI.Data;
using TareaAPI.Models;
using TareaAPI.Models.DTOs;

namespace TareaAPI.Repositories
{
    public class CategoriaRepository: ICategoriaRepository
    {
        private readonly AppDbContext _context;
        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<Categoria>> GetAllCategoriasAsync(PaginationQuery query)
        {
            
            var totalItems = await _context.Categorias.CountAsync();
            var items = await _context.Categorias.Skip((query.Page - 1) * query.PageSize)
                                               .Take(query.PageSize)
                                               .ToListAsync();
            return new PaginatedResponse<Categoria>(items, totalItems, query.Page, query.PageSize);
        }
        public async Task<Categoria?> GetCategoriaByIdAsync(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }
        public async Task<Categoria> CreateCategoriaAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }
        public async Task UpdateCategoriaAsync(Categoria categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCategoriaAsync(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}