namespace BO;

/// <summary>
/// defining order-for-list struct
/// </summary>

public class OrderForList
{
    /// <summary>
    /// the order-for-list's id
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// the order-for-list's customer's name
    /// </summary>
    public string? CustomerName { get; set; }
    /// <summary>
    /// the order-for-list's order's status
    /// </summary>
    public eOrderStatus? Status { get; set; }
    /// <summary>
    /// amount of items in the order-for-list
    /// </summary>
    public int AmountOfItems { get; set; }
    /// <summary>
    /// the order-for-list's total price
    /// </summary>
    public double TotalPrice { get; set; }


    
    /// <summary>
    /// overriding the ToString function for printing the order-for-list's details
    /// </summary>
    /// <returns> the to-string of the order-for-list</returns>
    public override string ToString() => $@"Order for list ID: {ID}, customer name: {CustomerName}, staus: {Status}, amount of items:{AmountOfItems},  total price:{TotalPrice}";

}
