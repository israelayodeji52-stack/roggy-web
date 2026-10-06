namespace Roggy.Application.Reports.RejectReport;

public sealed record RejectReportCommand(
    Guid ReportId,
    Guid ReviewerUserId);