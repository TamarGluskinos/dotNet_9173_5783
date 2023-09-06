namespace BO;

/// <summary>
/// defining order struct
/// </summary>

public class Order
{
    /// <summary>
    /// the order's id
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// the coustomer's name
    /// </summary>
    public string? CustomerName { get; set; }
    /// <summary>
    /// the coustomer's email
    /// </summary>
    public string? CustomerEmail { get; set; }
    /// <summary>
    /// the costomer's address
    /// </summary>
    public string? CustomerAddress { get; set; }
    /// <summary>
    /// the order's order date
    /// </summary>
    public DateTime? OrderDate { get; set; }
    /// <summary>
    /// the order's status
    /// </summary>
    public eOrderStatus? Status { get; set; }
    /// <summary>
    /// the order's ship date
    /// </summary>
    public DateTime? ShipDate { get; set; }
    /// <summary>
    /// the order's delivery date
    /// </summary>
    public DateTime? DeliveryDate { get; set; }
    /// <summary>
    /// the order's item list
    /// </summary>
    public List<BO.OrderItem?>? Items { get; set; } = new();
    /// <summary>
    /// the order's total price
    /// </summary>
    public double TotalPrice { get; set; }


    /// <summary>
    /// overriding the ToString function for printing the order's details
    /// </summary>
    /// <returns>the to-string of the order</returns>
    public override string ToString() { 
        string toString = 
            $@"order ID={ID},
            customer mame: {CustomerName}, 
            email {CustomerEmail}, 
            address {CustomerAddress}.
            order date: {OrderDate}, 
            ship date: {ShipDate}, 
            delivery date: {DeliveryDate}, 
            status: {Status}.
            total price:{TotalPrice}
            items:";
        Items?.ForEach(i => toString += "\n \t " + i);
        return toString;
    }

}

