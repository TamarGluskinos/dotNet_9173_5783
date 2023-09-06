namespace DO;

/// <summary>
/// defining order struct
/// </summary>
public struct Order
{
    public int ID { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerAddress { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ShipDate { get; set; }
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// overriding the ToString function for printing the order's details
    /// </summary>
    /// <returns>Order to string</returns>
    public override string ToString() => $@" Order: ID : {ID}, customer name: {CustomerName}, email: {CustomerEmail}, address: {CustomerAddress},   order date : {OrderDate}, ship : {ShipDate}, delivery : {DeliveryDate}";


}

