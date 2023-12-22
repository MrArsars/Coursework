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

    [HttpGet("add")]
    public async Task<IActionResult> AddRecord()
    {
        var state = SystemState.GetState();
        if (state) await _telegram.SendMessageAsync();
        return Ok(true);
    }
}