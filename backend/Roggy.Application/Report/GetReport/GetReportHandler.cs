using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Reports.GetReports;

public sealed class GetReportsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetReportsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Roggy.Domain.Entities.Report>> HandleAsync(
        GetReportsQuery query,
        CancellationToken cancellationToken = default)
    {
        var reportsQuery = _dbContext.Reports
            .AsNoTracking()
            .OrderByDescending(report => report.CreatedAt)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            var status = query.Status.Trim();

            reportsQuery = reportsQuery
                .Where(report => report.Status == status)
                .OrderByDescending(report => report.CreatedAt);
        }

        return await reportsQuery
            .ToListAsync(cancellationToken);
    }
}