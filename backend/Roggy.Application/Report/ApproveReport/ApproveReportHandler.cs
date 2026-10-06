using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Reports.ApproveReport;

public sealed class ApproveReportHandler
{
    private readonly IApplicationDbContext _dbContext;

    public ApproveReportHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Roggy.Domain.Entities.Report> HandleAsync(
        ApproveReportCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.ReportId == Guid.Empty)
        {
            throw new ArgumentException(
                "Report ID is required.",
                nameof(command.ReportId));
        }

        if (command.ReviewerUserId == Guid.Empty)
        {
            throw new ArgumentException(
                "Reviewer user ID is required.",
                nameof(command.ReviewerUserId));
        }

        var report = await _dbContext.Reports
            .FindAsync(
                [command.ReportId],
                cancellationToken);

        if (report is null)
        {
            throw new KeyNotFoundException(
                "Report was not found.");
        }

        var reviewer = await _dbContext.Users
            .FindAsync(
                [command.ReviewerUserId],
                cancellationToken);

        if (reviewer is null)
        {
            throw new KeyNotFoundException(
                "Reviewer user was not found.");
        }

        if (!string.Equals(
                reviewer.Role,
                "Admin",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException(
                "Only administrators can approve reports.");
        }

        if (!string.Equals(
                report.Status,
                "Pending",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Only pending reports can be approved.");
        }

        report.Approve(command.ReviewerUserId);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return report;
    }
}