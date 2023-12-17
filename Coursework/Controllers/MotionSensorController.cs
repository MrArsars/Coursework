using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Coursework.Controllers;

[ApiController]
[Route("MotionSensor")]
public class MotionSensorController: ControllerBase
{
    private readonly DataContext _context;

    public MotionSensorController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("add")]
    public async Task<IActionResult> AddRecord()
    {
        await _context.MotionSensorData.AddAsync(new MotionSensorDataModel()
        {
            Id = 0,
            Time = DateTime.Now,
            IsMovingDetected = true,
        });
        await _context.SaveChangesAsync();
        return Ok(true);
    }
}