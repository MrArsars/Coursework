using Coursework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Coursework.Controllers;

[ApiController]
[Route("MotionSensor")]
public class MotionSensorController: ControllerBase
{
    [HttpGet("switch")]
    public Task<IActionResult> Switch()
    {
        var state = SystemState.SwitchState();
        return Task.FromResult<IActionResult>(Ok(state));
    }

    [HttpGet("get")]
    public Task<IActionResult> GetState()
    {
        return Task.FromResult<IActionResult>(Ok(SystemState.GetState()));
    }

    [HttpGet("add")]
    public Task<IActionResult> AddRecord()
    {
        return Task.FromResult<IActionResult>(!SystemState.GetState() ? Ok(false) : Ok(true));
    }
}