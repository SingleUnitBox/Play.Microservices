using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Play.Operation.Api.Hubs;
using Play.Operation.Api.Infrastructure;

namespace Play.Operation.Api.Controllers;

// Example: Trigger operation events from a controller or service
public class CommandController : ControllerBase
{
    private readonly IHubContext<PlayHub> _hubContext;

    public CommandController(IHubContext<PlayHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("trigger/{userId}")]
    public async Task<IActionResult> TriggerOperation(string userId, [FromQuery] string status)
    {
        var group = Guid.Parse(userId).ToUserGroup();

        switch (status.ToLower())
        {
            case "pending":
                await _hubContext.Clients.Group(group).SendAsync("operation_pending", new { Id = Guid.NewGuid(), Status = "Pending" });
                break;
            case "completed":
                await _hubContext.Clients.Group(group).SendAsync("operation_completed", new { Id = Guid.NewGuid(), Status = "Completed" });
                break;
            case "rejected":
                await _hubContext.Clients.Group(group).SendAsync("operation_rejected", new { Id = Guid.NewGuid(), Status = "Rejected" });
                break;
            default:
                return BadRequest("Invalid status.");
        }

        return Ok("Operation triggered.");
    }
}
