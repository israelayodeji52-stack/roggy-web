namespace Roggy.Contracts.Reports;

public sealed record ReportResponse(
    Guid Id,
    Guid ReporterUserId,
    string TargetType,
    Guid TargetId,
    string Reason,
    string? Description,
    string Status,
    Guid? ReviewedByUserId,
    DateTime? ReviewedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt);