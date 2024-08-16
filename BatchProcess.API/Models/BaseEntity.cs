using System.Xml.Serialization;

namespace BatchProcess.Api.Models;

/// <summary>
/// Represents a base entity with common properties.
/// </summary>
[XmlRoot("BaseEntity")]
public class BaseEntity
{
    /// <summary>
    /// Gets or sets the creator of the record.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was last updated.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }
}
