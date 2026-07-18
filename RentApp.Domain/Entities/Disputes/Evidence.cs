using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Disputes
{
    public class Evidence : BaseEntity
    {
        public Guid DisputeId { get; private set; }
        public string FileUrl { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        private Evidence() { } // EF Core

        public Evidence(Guid disputeId, string fileUrl, string? description)
        {
            DisputeId = disputeId;
            FileUrl = fileUrl;
            Description = description;
        }
    }
}
