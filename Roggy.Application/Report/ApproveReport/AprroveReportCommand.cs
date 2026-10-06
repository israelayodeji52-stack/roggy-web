namespace Roggy.Application.Reports.ApproveReport;

public sealed record ApproveReportCommand(
    Guid ReportId,
    Guid ReviewerUserId);