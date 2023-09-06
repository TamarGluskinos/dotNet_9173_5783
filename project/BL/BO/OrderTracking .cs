namespace BO;

/// <summary>
/// defining order-tracking struct
/// </summary>

public class OrderTracking
{
    /// <summary>
    /// the order-tracking's ID
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// the order-tracking's status
    /// </summary>
    public eOrderStatus? Status { get; set; }
    /// <summary>
    /// the track-list
    /// </summary>
    public List<Tuple<DateTime?, eOrderStatus?>>? TrackList { get; set; } = new();

    /// <summary>
    /// overriding the ToString function for printing the order-tracking's details
    /// </summary>
    /// <returns>the to-string of order tracking</returns>
    public override string ToString()
    {
        string toString = 
            $@"ID: {ID}, 
            Status: {Status}, 
            dates: ";
        TrackList?.ForEach(i => toString += "\n \t \t" + i.Item2+" on "  + i.Item1);
        return toString;
    }
}
