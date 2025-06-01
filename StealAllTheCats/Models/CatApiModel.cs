namespace StealAllTheCats.Models;

/// <summary>
/// Represents the cat data model returned by the external Cat API.
/// </summary>
public class CatApiModel
{
    /// <summary>
    /// Gets or sets the external cat ID.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the image URL.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the image width.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the image height.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the list of breeds.
    /// </summary>
    public List<Breed> Breeds { get; set; } = new();

    /// <summary>
    /// Represents a breed object from the Cat API.
    /// </summary>
    public class Breed
    {
        /// <summary>
        /// Gets or sets the temperament string.
        /// </summary>
        public string? Temperament { get; set; }
    }
}
