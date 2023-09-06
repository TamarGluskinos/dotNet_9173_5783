namespace Simulator;

/// <summary>
/// Simulator class
/// </summary>
public static class Simulator
{
    /// <summary>
    /// stop simulation event
    /// </summary>
    public static event EventHandler? StopSimulator;

    /// <summary>
    /// update progress event
    /// </summary>
    public static event EventHandler? UpdateProgress;

    /// <summary>
    /// boolean variable to know whether to stop
    /// </summary>
    private static bool stop { get; set; } = false;

    /// <summary>
    /// BL instance
    /// </summary>
    private static BlApi.IBL Bl = BlApi.Factory.Get();

    /// <summary>
    /// order ID
    /// </summary>
    private static int? orderId { get; set; }

    /// <summary>
    /// randomizer
    /// </summary>
    private static Random rand = new Random();

    /// <summary>
    /// simulator details
    /// </summary>
    private static SimulatorEventDetails? details { get; set; }

    /// <summary>
    /// runs thread for simulator
    /// </summary>
    public static void run()
    {
        new Thread(() =>
                 {
                     while (!stop)
                     {
                         orderId = Bl.Order.ChooseOrder();
                         if (orderId == null)
                         {
                             OnStopSimulator();
                             break;
                         }
                         int time = rand.Next(5, 10);
                         BO.Order order = Bl.Order.ReadOrderProperties((int)orderId);
                         details = new SimulatorEventDetails(time, order);
                         OnUpdateProgress();
                         Thread.Sleep(1000 * details.time);
                         if (Bl.Order.ReadOrderProperties((int)orderId).Status == BO.eOrderStatus.Ordered)
                             Bl.Order.UpdateOrderSent((int)orderId);
                         else
                             Bl.Order.UpdateOrderDelivery((int)orderId);
                     }
                 }).Start();
    }

    /// <summary>
    /// stops simulator
    /// </summary>
    private static void OnStopSimulator()
    {
        if (StopSimulator != null)
            StopSimulator(null, EventArgs.Empty);
        stop = true;
    }

    /// <summary>
    /// updates progress
    /// </summary>
    private static void OnUpdateProgress()
    {
        if (UpdateProgress != null)
            UpdateProgress(null, details ?? EventArgs.Empty);
    }

    /// <summary>
    /// stops simulator
    /// </summary>
    public static void ToStop()
    {
        stop = true;
    }
}
