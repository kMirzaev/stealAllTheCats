namespace StealAllTheCats.Entities;

/// <summary>
/// Represents a cat entity stored in the database.
/// </summary>
public class CatEntity
{
    /// <summary>
    /// Gets or sets the database ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the external cat API ID.
    /// </summary>
    public string CatId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the image width.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the image height.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the image URL.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty; 

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the tags associated with the cat.
    /// </summary>
    public ICollection<TagEntity> Tags { get; set; } = new List<TagEntity>();
}
