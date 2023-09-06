namespace Simulator;

/// <summary>
/// simulator event details
/// </summary>
public class SimulatorEventDetails:EventArgs
{
    /// <summary>
    /// number of seconds for the simulator to run
    /// </summary>
    public int time { get; set; }

    /// <summary>
    /// order for the simulator to deal with
    /// </summary>
    public BO.Order? Order { get; set; }

    /// <summary>
    /// SimulatorEventDetails constructor
    /// </summary>
    /// <param name="_time">seconds</param>
    /// <param name="_order">order</param>
    public SimulatorEventDetails(int _time, BO.Order? _order)
    {
        time = _time;
        Order = _order;
    }
}
