using Coursework.Models;
using Coursework.Services;
using Microsoft.AspNetCore.Mvc;

namespace Coursework.Controllers;

[ApiController]
[Route("MotionSensor")]
public class MotionSensorController : ControllerBase
{
    private readonly TelegramBotService _telegram;

    public MotionSensorController(TelegramBotService telegram)
    {
        _telegram = telegram;
    }

    // [HttpGet("switch")]
    // public Task<IActionResult> Switch()
    // {
    //     var state = SystemState.SwitchState();
    //     return Task.FromResult<IActionResult>(Ok(state));
    // }
    //
    // [HttpGet("get")]
    // public Task<IActionResult> GetState()
    // {
    //     return Task.FromResult<IActionResult>(Ok(SystemState.GetState()));
    // }

    [HttpGet("add")]
    public async Task<IActionResult> AddRecord()
    {
        var state = SystemState.GetState();
        if (state) await _telegram.SendMessageAsync();
        return !state ? Ok(false) : Ok(true);
    }
}