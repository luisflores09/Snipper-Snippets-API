using Microsoft.AspNetCore.Mvc;
using Snipper_Snippets_API.Models;
using Snipper_Snippets_API.Services;

[ApiController]
[Route("[controller]")]
public class SnippetsController : ControllerBase
{
    private readonly SnippetService _snippetService;

    public SnippetsController(SnippetService snippetService)
    {
        _snippetService = snippetService;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] string? lang = null)
    {
        var snippets = _snippetService.GetAll(lang);
        return Ok(snippets);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var snippet = _snippetService.GetById(id);
        return snippet is not null ? Ok(snippet) : NotFound();
    }

    [HttpPost]
    public IActionResult AddSnippet([FromBody] Snippet snippet)
    {
        var createdSnippet = _snippetService.AddSnippet(snippet);
        return CreatedAtAction(nameof(GetById), new { id = createdSnippet.Id }, createdSnippet);
    }
}