namespace Coursework.Models;

public class MotionSensorDataModel
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public bool IsMovingDetected { get; set; }
}