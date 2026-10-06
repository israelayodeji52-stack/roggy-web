using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class Report : BaseEntity
{
    public Guid ReporterUserId { get; private set; }

    public string TargetType { get; private set; } = string.Empty;

    public Guid TargetId { get; private set; }

    public string Reason { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string Status { get; private set; } = "Pending";

    public Guid? ReviewedByUserId { get; private set; }

    public DateTime? ReviewedAt { get; private set; }

    public User Reporter { get; private set; } = null!;

    private Report()
    {
    }

    public Report(
        Guid reporterUserId,
        string targetType,
        Guid targetId,
        string reason,
        string? description = null)
    {
        ReporterUserId = reporterUserId;
        TargetType = targetType;
        TargetId = targetId;
        Reason = reason;
        Description = description;

        Status = "Pending";
    }

    public void Approve(
        Guid reviewerUserId)
    {
        Status = "Approved";
        ReviewedByUserId = reviewerUserId;
        ReviewedAt = DateTime.UtcNow;

        UpdateTimestamp();
    }

    public void Reject(
        Guid reviewerUserId)
    {
        Status = "Rejected";
        ReviewedByUserId = reviewerUserId;
        ReviewedAt = DateTime.UtcNow;

        UpdateTimestamp();
    }
}