using System.Security.Claims;
using api.Data;
using api.Dtos;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INotesService _notesService;

    public NotesController(INotesService notesService)
    {
        _notesService = notesService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var notesResult = await _notesService.GetAllNotes(query);
        var notesAmount = await _notesService.GetAllNotesCount();
        return Ok(new
        {
            notes = notesResult,
            amount = notesAmount
        });
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var noteResult = await _notesService.GetNoteById(id);
        return noteResult is null ? NotFound() : Ok(noteResult);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateNoteRequestDto createNoteDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var noteResult = await _notesService.CreateNote(createNoteDto);
        if (noteResult is null) return BadRequest("Something went wrong.");
        return CreatedAtAction(nameof(GetById), new { id = noteResult.Id }, noteResult);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateNoteRequestDto updateNoteDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var noteResult = await _notesService.UpdateNote(id, updateNoteDto);
        return noteResult is null ? BadRequest(new {id , updateNoteDto}) : Ok(noteResult);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var noteResult = await _notesService.DeleteNote(id);
        return noteResult is null ? NotFound() : Ok(noteResult);
    }
    
    

}