namespace BO;

/// <summary>
/// defining order-item struct
/// </summary>

public class OrderItem
 {
    /// <summary>
    /// the order-item order's id
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// the order-item product's id
    /// </summary>
    public int ProductID { get; set; }
    /// <summary>
    /// the order-item product's name
    /// </summary>
    public string? ProductName { get; set; }
    /// <summary>
    /// the order-item price
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// the amount of the order-item
    /// </summary>
    public int Amount { get; set; }
    /// <summary>
    /// the total price or the order-item
    /// </summary>
    public double TotalPrice { get; set; }

    
    /// <summary>
    /// overriding the ToString function for printing the order-item's details
    /// </summary>
    /// <returns>the to-string of the order-item</returns>
    public override string ToString() => $@" ID: {ID}, ProductName: {ProductName}, ProductID: {ProductID}, Amount: {Amount},  Price: {Price}, TotalPrice: {TotalPrice}";

}

