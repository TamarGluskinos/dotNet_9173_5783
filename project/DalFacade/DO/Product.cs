namespace DO;
/// <summary>
/// defining product struct
/// </summary>

public struct Product
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public eCategories? Category { get; set; }
    public int InStock { get; set; }

    /// <summary>
    /// overriding the ToString function for printing the product's details
    /// </summary>
    /// <returns>product to string</returns>
    public override string ToString() => $@"Product ID={ID}: {Name}, category - {Category} 	Price: {Price} Amount in stock: {InStock}";

}

