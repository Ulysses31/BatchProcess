using System;
using System.Xml.Serialization;

namespace BatchProcess.Api.Models.Entities
{
    /// <summary>
    /// Represents a PostDto object.
    /// </summary>
    public class PostDto : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Post_Id.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the UserId.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the Body.
        /// </summary>
        public string? Body { get; set; }
    }
}
