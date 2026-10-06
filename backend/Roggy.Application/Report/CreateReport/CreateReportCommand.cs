namespace Roggy.Application.Reports.CreateReport;

public sealed record CreateReportCommand(
    Guid ReporterUserId,
    string TargetType,
    Guid TargetId,
    string Reason,
    string? Description = null);