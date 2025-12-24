using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareaAPI.Data;
using TareaAPI.Models;
using TareaAPI.Models.DTOs;

namespace TareaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TareasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IValidator<TareaCreateDto> _tareaValidator;
    private readonly IValidator<TareaUpdateDto> _updateValidator;

    public TareasController(AppDbContext context, IValidator<TareaCreateDto> tareaValidator, IValidator<TareaUpdateDto> updateValidator)
    {
        _context = context;
        _tareaValidator = tareaValidator;
        _updateValidator = updateValidator;
    }

    // GET: api/tareas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas()
    {
        return await _context.Tareas.ToListAsync();
    }

    // GET: api/tareas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Tarea>> GetTarea(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea == null) return NotFound();
        return tarea;
    }

    // POST: api/tareas
    [HttpPost]
    public async Task<ActionResult<Tarea>> PostTarea([FromBody] TareaCreateDto createDTO)
    {
        var validation = await _tareaValidator.ValidateAsync(createDTO);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var tarea = new Tarea
        {
           Nombre = createDTO.Nombre,
           Completada = createDTO.Completada,
            FechaCreacion = DateTime.Now
        };

        _context.Tareas.Add(tarea);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id },
        new TareaDto {Nombre = tarea.Nombre ?? "", Completada = tarea.Completada});
    }

    // PUT: api/tareas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTarea(int id, Tarea tarea)
    {
        if (id != tarea.Id) return BadRequest();
        _context.Entry(tarea).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TareaExists(id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    // DELETE: api/tareas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTarea(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea == null) return NotFound();
        _context.Tareas.Remove(tarea);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool TareaExists(int id) => _context.Tareas.Any(e => e.Id == id);
}
