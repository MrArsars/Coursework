namespace Coursework.Models;

public static class SystemState
{
    private static bool State { get; set; }

    public static bool GetState()
    {
        return State;
    }

    public static bool SwitchState()
    {
        State = !State;
        return State;
    }
}