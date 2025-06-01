namespace StealAllTheCats.Entities;

/// <summary>
/// Represents a tag entity associated with cats.
/// </summary>
public class TagEntity
{
    /// <summary>
    /// Gets or sets the database ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the tag name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the collection of cats associated with this tag.
    /// </summary>
    public ICollection<CatEntity> Cats { get; set; } = new List<CatEntity>();
}
