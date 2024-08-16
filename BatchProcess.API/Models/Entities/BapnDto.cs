using System;
using System.Text.Json.Serialization;

namespace BatchProcess.Api.Models.Entities
{
    /// <summary>
    /// Represents a BapnDto object.
    /// </summary>
    public class BapnDto : BaseEntity
    {
        /// <summary>
        /// Gets or sets the BapN_Id.
        /// </summary>
        public Guid BapN_Id { get; set; }

        /// <summary>
        /// Gets or sets the BapN_BapId.
        /// </summary>
        public Guid BapN_BapId { get; set; }

        /// <summary>
        /// Gets or Sets the current BapDto
        /// </summary>
        public BapDto? BapN_BapDto { get; set; }

        /// <summary>
        /// Gets or sets the BapN_AA.
        /// </summary>
        public int BapN_AA { get; set; }

        /// <summary>
        /// Gets or sets the BapN_DateTime.
        /// </summary>
        public DateOnly? BapN_DateTime { get; set; }

        /// <summary>
        /// Gets or sets the BapN_kind.
        /// </summary>
        public int BapN_kind { get; set; }

        /// <summary>
        /// Gets or sets the BapN_Data.
        /// </summary>
        public string? BapN_Data { get; set; }

    }
}
