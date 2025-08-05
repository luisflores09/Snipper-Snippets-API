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
        var user = HttpContext.Items["User"] as UserResponse;
        var snippets = _snippetService.GetAll(lang, user?.Id);
        return Ok(snippets);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = HttpContext.Items["User"] as UserResponse;
        var snippet = _snippetService.GetById(id, user?.Id);
        return snippet is not null ? Ok(snippet) : NotFound();
    }

    [HttpPost]
    public IActionResult AddSnippet([FromBody] CreateSnippetRequest request)
    {
        var user = HttpContext.Items["User"] as UserResponse;
        if (user == null) return Unauthorized();

        var createdSnippet = _snippetService.AddSnippet(request, user.Id);
        return CreatedAtAction(nameof(GetById), new { id = createdSnippet.Id }, createdSnippet);
    }
}