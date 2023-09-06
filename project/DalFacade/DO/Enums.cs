
namespace DO;

/// <summary>
/// enum for the categories in the store
/// </summary>
public enum eCategories
{
    Breads,
    Milky,
    Treats,
    Fruit,
    Veg
}

/// <summary>
/// enum for the options of the different classes for handling
/// </summary>
public enum eOptions
{
    Exit,
    Orders,
    OrderItems,
    Products
}

/// <summary>
/// enum for the options of the different actions on orders and products
/// </summary>
public enum eCrudOptions
{
    Create = 1,
    ReadSingle,
    ReadAll,
    Update,
    Delete
}

/// <summary>
/// enum for the options of the different actions on order items
/// </summary>
public enum eOrderItems
{
    Create = 1,
    ReadById,
    ReadByOrderProductIds,
    ReadByOrderId,
    ReadAll,
    Update,
    Delete
}
