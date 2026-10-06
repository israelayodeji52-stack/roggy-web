using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Reports.CreateReport;

public sealed class CreateReportHandler
{
    private readonly IApplicationDbContext _dbContext;

    public CreateReportHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Roggy.Domain.Entities.Report> HandleAsync(
        CreateReportCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.ReporterUserId == Guid.Empty)
        {
            throw new ArgumentException(
                "Reporter user ID is required.",
                nameof(command.ReporterUserId));
        }

        if (string.IsNullOrWhiteSpace(command.TargetType))
        {
            throw new ArgumentException(
                "Target type is required.",
                nameof(command.TargetType));
        }

        if (command.TargetId == Guid.Empty)
        {
            throw new ArgumentException(
                "Target ID is required.",
                nameof(command.TargetId));
        }

        if (string.IsNullOrWhiteSpace(command.Reason))
        {
            throw new ArgumentException(
                "Report reason is required.",
                nameof(command.Reason));
        }

        var user = await _dbContext.Users
            .FindAsync(
                [command.ReporterUserId],
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "Reporter user was not found.");
        }

        var report = new Roggy.Domain.Entities.Report(
            command.ReporterUserId,
            command.TargetType.Trim(),
            command.TargetId,
            command.Reason.Trim(),
            command.Description?.Trim());

        _dbContext.Reports.Add(report);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return report;
    }
}