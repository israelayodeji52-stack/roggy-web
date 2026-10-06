using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.Reports.ApproveReport;
using Roggy.Application.Reports.CreateReport;
using Roggy.Application.Reports.GetReports;
using Roggy.Application.Reports.RejectReport;
using Roggy.Contracts.Reports;
using Roggy.Domain.Entities;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly CreateReportHandler _createReportHandler;
    private readonly ApproveReportHandler _approveReportHandler;
    private readonly RejectReportHandler _rejectReportHandler;
    private readonly GetReportsHandler _getReportsHandler;

    public ReportsController(
        CreateReportHandler createReportHandler,
        ApproveReportHandler approveReportHandler,
        RejectReportHandler rejectReportHandler,
        GetReportsHandler getReportsHandler)
    {
        _createReportHandler = createReportHandler;
        _approveReportHandler = approveReportHandler;
        _rejectReportHandler = rejectReportHandler;
        _getReportsHandler = getReportsHandler;
    }

    [HttpPost]
    public async Task<ActionResult<ReportResponse>> Create(
        [FromBody] CreateReportCommand request,
        CancellationToken cancellationToken)
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized(new
            {
                message = "Authenticated user ID is missing."
            });
        }

        try
        {
            var report =
                await _createReportHandler.HandleAsync(
                    new CreateReportCommand(
                        userId,
                        request.TargetType,
                        request.TargetId,
                        request.Reason,
                        request.Description),
                    cancellationToken);

            return Ok(ToResponse(report));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<ReportResponse>>> GetReports(
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        try
        {
            var reports =
                await _getReportsHandler.HandleAsync(
                    new GetReportsQuery(status),
                    cancellationToken);

            var response = reports
                .Select(ToResponse)
                .ToList();

            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpPost("{reportId:guid}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReportResponse>> Approve(
        Guid reportId,
        CancellationToken cancellationToken)
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized(new
            {
                message = "Authenticated user ID is missing."
            });
        }

        try
        {
            var report =
                await _approveReportHandler.HandleAsync(
                    new ApproveReportCommand(
                        reportId,
                        userId),
                    cancellationToken);

            return Ok(ToResponse(report));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
            {
                message = exception.Message
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost("{reportId:guid}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReportResponse>> Reject(
        Guid reportId,
        CancellationToken cancellationToken)
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized(new
            {
                message = "Authenticated user ID is missing."
            });
        }

        try
        {
            var report =
                await _rejectReportHandler.HandleAsync(
                    new RejectReportCommand(
                        reportId,
                        userId),
                    cancellationToken);

            return Ok(ToResponse(report));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
            {
                message = exception.Message
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    private static ReportResponse ToResponse(
        Report report)
    {
        return new ReportResponse(
            report.Id,
            report.ReporterUserId,
            report.TargetType,
            report.TargetId,
            report.Reason,
            report.Description,
            report.Status,
            report.ReviewedByUserId,
            report.ReviewedAt,
            report.CreatedAt,
            report.UpdatedAt);
    }
}