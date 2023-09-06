namespace BO;

/// <summary>
/// defining product struct
/// </summary>

public class Product
{
    /// <summary>
    /// the product's ID
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// the product's name
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// the product's price
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// the product's category
    /// </summary>
    public eCategories? Category { get; set; }
    /// <summary>
    /// the amount in stock of the prodcut
    /// </summary>
    public int InStock { get; set; }

    /// <summary>
    /// overriding the ToString function for printing the product's details
    /// </summary>
    /// <returns>the to-string of the product</returns>
    public override string ToString() => $@"Product ID: {ID}, Name: {Name},Price: {Price}, Category: {Category},  Instock: {InStock}";

}


