namespace BO;

/// <summary>
/// defining cart struct
/// </summary>

public class Cart
{
    /// <summary>
    /// the customer's name
    /// </summary>
    public string? CustomerName { get; set; }
    /// <summary>
    /// the customer's email address
    /// </summary>
    public string? CustomerEmail { get; set; }
    /// <summary>
    /// the customer's address
    /// </summary>
    public string? CustomerAddress { get; set; }
    /// <summary>
    /// the list of items in the cart
    /// </summary>
    public List<OrderItem?>? Items { get; set; } = new List<OrderItem>();
    /// <summary>
    /// the price of the items in the cart
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// overriding the ToString function for printing the cart's details
    /// </summary>
    /// <returns>the to-string of the cart</returns>
    public override string ToString()
    {
        string toString = 
    $@"Cart: customer mame {CustomerName}, 
    email {CustomerEmail}, address {CustomerAddress}. 
    total price {Price} items: ";
        Items?.ForEach(i => toString += "\n \t " + i);
        return toString;
    }
}

