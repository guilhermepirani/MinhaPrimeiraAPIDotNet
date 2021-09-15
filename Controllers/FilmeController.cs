using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesAPI.Controllers {

  [ApiController]
  [Route("[controller]")]
  public class FilmeController : ControllerBase {

    // Criando referência ao banco de dados e ao AutoMapper.
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper) {
      _context = context;
      _mapper = mapper;
    }

    // Adicionar Filme ao banco.
    [HttpPost]
    public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto) {
      Filme filme = _mapper.Map<Filme>(filmeDto);

      _context.Filmes.Add(filme);
      _context.SaveChanges();
      return CreatedAtAction(nameof(RecuperarFilmesPorId), new { Id = filme.Id }, filme);
    }

    // Retornar todos os filmes do banco.
    [HttpGet]
    public IActionResult RecuperarFilmes() {
      return Ok(_context.Filmes);
    }

    // Retornar filme pelo id.
    [HttpGet("{id}")]
    public IActionResult RecuperarFilmesPorId(int id) {
      Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
      if (filme != null) {
        ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        filmeDto.HoraDaConsulta = DateTime.Now;
        return Ok(filmeDto);
      }
      return NotFound();
    }

    // Atualizar uma entrada no DB.
    [HttpPut("{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDto filmeDto) {
      Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
      if (filme == null) {
        return NotFound();
      }
      _mapper.Map(filmeDto, filme);
      _context.SaveChanges();
      return NoContent();
    }

    // Deletar uma entrada no DB.
    [HttpDelete("{id}")]
    public IActionResult DeletarFilme(int id) { 
      Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
      if (filme == null) {
        return NotFound();
      }
      _context.Remove(filme);
      _context.SaveChanges();
      return NoContent();
    }
  }
}
