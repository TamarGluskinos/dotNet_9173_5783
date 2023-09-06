namespace BO;

/// <summary>
/// defining product-item struct
/// </summary>

public class ProductItem
 {
    /// <summary>
    /// the product-item's ID
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// the product-item's name
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// the product-item's price
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// the product-item's category
    /// </summary>
    public eCategories? Category { get; set; }
    /// <summary>
    /// checks if the product-item is in stock
    /// </summary>
    public bool IsInStock { get; set; }
    /// <summary>
    /// the amount of the product-item
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// overriding the ToString function for printing the product-item's details
    /// </summary>
    /// <returns>the to-string of the product-item</returns>
    public override string ToString() => $@"ID: {ID}, Name: {Name}, Price: {Price}, Category: {Category}, Amount : {Amount}, Is available: {IsInStock}";
}

