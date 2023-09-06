namespace BO;

/// <summary>
/// defining product-for-list struct
/// </summary>

public class ProductForList
{
    /// <summary>
    /// the product-for-list's ID
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// the product-for-list's name
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// the product-for-list's price
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// the product-for-list's category
    /// </summary>
    public eCategories? Category { get; set; }

    /// <summary>
    /// overriding the ToString function for printing the product-for-list's details
    /// </summary>
    /// <returns>the to-string of the product-for-list</returns>
    public override string ToString() => $@"ID: {ID}, Name: {Name}, Price: {Price}, Category: {Category}";
}
