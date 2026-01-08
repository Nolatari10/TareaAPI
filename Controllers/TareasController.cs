using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TareaAPI.Models;
using TareaAPI.Models.DTOs;
using TareaAPI.Repositories;

namespace TareaAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize] 
public class TareasController : ControllerBase
{
    private readonly IValidator<TareaCreateDto> _tareaValidator;
    private readonly IValidator<TareaUpdateDto> _updateValidator;
    private readonly ITareaRepository _repository;

    public TareasController( IValidator<TareaCreateDto> tareaValidator, IValidator<TareaUpdateDto> updateValidator, ITareaRepository repository)
    {
      
        _tareaValidator = tareaValidator;
        _updateValidator = updateValidator;
        _repository = repository;
    }

    // GET: api/tareas
    //PAGINATION ADDED
    
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<Tarea>>> GetTareas([FromQuery] PaginationQuery paginationQuery)
    {
        //all responsability moved to repository
       var response = await _repository.GetAllTareasAsync(paginationQuery);
       return Ok(response);
    }

    // GET: api/tareas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Tarea>> GetTarea(int id)
    {
        var tarea = await _repository.GetTareaByIdAsync(id);
        if (tarea == null) return NotFound();
        return Ok(tarea);
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
            FechaCreacion = DateTime.Now,
            CategoriaId = createDTO.CategoriaId //ADDED
        };

        var tareaCreada = await _repository.CreateTareaAsync(tarea);

        return CreatedAtAction("GetTarea", new { id = tareaCreada.Id }, tareaCreada);
    }

    // PUT: api/tareas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTarea(int id, [FromBody] TareaUpdateDto updateDTO)
    {
        var validation = await _updateValidator.ValidateAsync(updateDTO);
        if (!validation.IsValid)
          return BadRequest(validation.Errors);

        var tarea = await _repository.GetTareaByIdAsync(id);
        if (tarea == null) return NotFound();

        if(updateDTO.Nombre != null)
            tarea.Nombre = updateDTO.Nombre;
        tarea.Completada = updateDTO.Completada;
        tarea.CategoriaId = updateDTO.CategoriaId; //ADDED allow change category

        await _repository.UpdateTareaAsync(tarea);
        return NoContent();
    }

    // DELETE: api/tareas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTarea(int id)
    {
        var tarea = await _repository.GetTareaByIdAsync(id);

        //validate if it exists
        if(tarea == null)
            return NotFound();
            
        //proceed to delete
        await _repository.DeleteTareaAsync(tarea);
        return NoContent();
    }

    private bool TareaExists(int id) => _repository.GetTareaByIdAsync(id) != null;
}
