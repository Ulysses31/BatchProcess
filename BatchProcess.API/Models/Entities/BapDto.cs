using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace BatchProcess.Api.Models.Entities
{
    /// <summary>
    /// Represents a BapDto object.
    /// </summary>
    public class BapDto : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Bap_Id.
        /// </summary>
        public Guid Bap_Id { get; set; }

        /// <summary>
        /// Gets or sets the Bap_Code.
        /// </summary>
        public string Bap_Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Bap_State.
        /// </summary>
        public int Bap_State { get; set; }

        /// <summary>
        /// Gets or sets the Bap_Started_DateTime.
        /// </summary>
        public DateOnly? Bap_Started_DateTime { get; set; }

        /// <summary>
        /// Gets or sets the Bap_Cancelled_DateTime.
        /// </summary>
        public DateOnly? Bap_Cancelled_DateTime { get; set; }

        /// <summary>
        /// Gets or sets the Bap_Finished_DateTime.
        /// </summary>
        public DateOnly? Bap_Finished_DateTime { get; set; }

        /// <summary>
        /// Gets or sets the Bap_Failed_DateTime.
        /// </summary>
        public DateOnly? Bap_Failed_DateTime { get; set; }

        /// <summary>
        /// Gets or sets the Bap_Session_Id.
        /// </summary>
        public Guid? Bap_Session_Id { get; set; }

        /// <summary>
        /// Gets or sets the BapN list
        /// </summary>
        public ICollection<BapnDto>? Bap_BapNs { get; set; } = new List<BapnDto>();
    }
}
