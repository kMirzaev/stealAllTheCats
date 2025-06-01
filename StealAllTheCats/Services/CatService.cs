using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Data;
using StealAllTheCats.Entities;
using StealAllTheCats.Models;

namespace StealAllTheCats.Services;

/// <summary>
/// Provides operations for fetching and managing cat entities.
/// </summary>
public class CatService
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CatService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="context">The database context.</param>
    public CatService(HttpClient httpClient, ApplicationDbContext context)
    {
        _httpClient = httpClient;
        _context = context;
    }

    /// <summary>
    /// Fetches 25 cats from the external API and saves new ones to the database.
    /// </summary>
    /// <returns>The list of newly added cats.</returns>
    public async Task<List<CatEntity>> FetchCatsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<CatApiModel>>(
            "https://api.thecatapi.com/v1/images/search?limit=25&has_breeds=1");

        if (response is null) return new List<CatEntity>();

        var existingCatIds = _context.Cats.Select(c => c.CatId).ToHashSet();
        var newCats = new List<CatEntity>();

        foreach (var cat in response.Where(c => !existingCatIds.Contains(c.Id)))
        {
            var temperament = cat.Breeds?.FirstOrDefault()?.Temperament;

            var tags = new List<TagEntity>();
            if (!string.IsNullOrWhiteSpace(temperament))
            {
                foreach (var tagName in temperament.Split(',', StringSplitOptions.TrimEntries))
                {
                    var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                    tags.Add(existingTag ?? new TagEntity { Name = tagName });
                }
            }

            newCats.Add(new CatEntity
            {
                CatId = cat.Id,
                Width = cat.Width,
                Height = cat.Height,
                ImageUrl = cat.Url,
                Tags = tags
            });
        }

        _context.Cats.AddRange(newCats);
        await _context.SaveChangesAsync();

        return newCats;
    }

    /// <summary>
    /// Retrieves a cat by its database ID.
    /// </summary>
    /// <returns>The cat entity, or null if not found.</returns>
    public async Task<CatEntity?> GetCatByCatIdAsync(string catId)
    {
        return await _context.Cats
            .Include(c => c.Tags)
            .FirstOrDefaultAsync(c => c.CatId == catId);
    }

    /// <summary>
    /// Retrieves cats with optional tag filter and paging support.
    /// </summary>
    /// <param name="tag">Optional tag to filter cats.</param>
    /// <param name="page">Page number.</param>
    /// <param name="pageSize">Page size.</param>
    /// <returns>List of cats matching the criteria.</returns>
    public async Task<List<CatEntity>> GetCatsAsync(string? tag, int page, int pageSize)
    {
        var query = _context.Cats.Include(c => c.Tags).AsQueryable();

        if (!string.IsNullOrWhiteSpace(tag))
        {
            query = query.Where(c => c.Tags.Any(t => t.Name == tag));
        }

        return await query
            .OrderByDescending(c => c.Created)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
