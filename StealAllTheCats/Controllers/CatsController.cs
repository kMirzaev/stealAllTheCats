using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Entities;
using StealAllTheCats.Services;

namespace StealAllTheCats.Controllers;

/// <summary>
/// Handles cat-related API endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CatsController : ControllerBase
{
    private readonly CatService _catService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CatsController"/> class.
    /// </summary>
    /// <param name="catService">The cat service.</param>
    public CatsController(CatService catService)
    {
        _catService = catService;
    }

    /// <summary>
    /// Fetches 25 cat images from TheCatAPI and saves them to the database (no duplicates).
    /// </summary>
    /// <returns>The list of newly added cats.</returns>
    [HttpPost("fetch")]
    public async Task<ActionResult<List<CatEntity>>> FetchCats()
    {
        var cats = await _catService.FetchCatsAsync();
        return Ok(cats);
    }

    /// <summary>
    /// Retrieves a cat by its external Cat API ID.
    /// </summary>
    /// <param name="catId">The external cat API ID.</param>
    /// <returns>The cat entity, or 404 if not found.</returns>
    [HttpGet("{catId}")]
    public async Task<ActionResult<CatEntity>> GetCatByCatId(string catId)
    {
        if (string.IsNullOrWhiteSpace(catId))
            return BadRequest("catId is required.");

        var cat = await _catService.GetCatByCatIdAsync(catId);
        if (cat == null)
            return NotFound();

        return Ok(cat);
    }

    /// <summary>
    /// Retrieves cats with optional tag filter and paging support.
    /// </summary>
    /// <param name="tag">Optional tag to filter cats.</param>
    /// <param name="page">Page number (default 1).</param>
    /// <param name="pageSize">Page size (default 10, max 100).</param>
    /// <returns>Paged list of cats.</returns>
    [HttpGet]
    public async Task<ActionResult<List<CatEntity>>> GetCats(
        [FromQuery] string? tag,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // Validation
        if (page <= 0) return BadRequest("Page must be greater than 0.");
        if (pageSize <= 0 || pageSize > 100) return BadRequest("PageSize must be between 1 and 100.");

        var cats = await _catService.GetCatsAsync(tag, page, pageSize);
        return Ok(cats);
    }
}