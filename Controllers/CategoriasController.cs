
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TareaAPI.Models;
using TareaAPI.Models.DTOs;
using TareaAPI.Repositories;

namespace TareaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IValidator<CategoriaCreateDto> _categoriaValidator;
        private readonly IValidator<CategoriaUpdateDto> _updateValidator;
        private readonly ICategoriaRepository _repository;

        public CategoriasController(IValidator<CategoriaCreateDto> categoriaValidator, IValidator<CategoriaUpdateDto> updateValidator, ICategoriaRepository repository)
        {
            _categoriaValidator = categoriaValidator;
            _updateValidator = updateValidator;
            _repository = repository;
        }        

        [HttpGet]
        public async Task<IActionResult> GetAllCategorias([FromQuery] PaginationQuery query)
        {
            var categorias = await _repository.GetAllCategoriasAsync(query);
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            var categoria = await _repository.GetCategoriaByIdAsync(id);
            if (categoria == null) 
                 return NotFound();
            return Ok(categoria);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostCategoria([FromBody] CategoriaCreateDto categoriaDto)
        {
            var validationResult = await _categoriaValidator.ValidateAsync(categoriaDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var categoria = new Categoria
            {
                Nombre = categoriaDto.Nombre,
                Descripcion = categoriaDto.Descripcion
            };

            await _repository.CreateCategoriaAsync(categoria);
            return CreatedAtAction(nameof(GetCategoriaById), new { id = categoria.categoriaId }, categoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, [FromBody] CategoriaUpdateDto categoriaDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(categoriaDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var categoria = await _repository.GetCategoriaByIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            if(categoriaDto.Nombre != null)
                categoria.Nombre = categoriaDto.Nombre;
            categoria.Descripcion = categoriaDto.Descripcion;

            await _repository.UpdateCategoriaAsync(categoria);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _repository.GetCategoriaByIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            await _repository.DeleteCategoriaAsync(categoria);
            return NoContent();
        }
    }
}