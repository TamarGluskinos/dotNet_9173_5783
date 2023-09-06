namespace BO;

/// <summary>
/// enum for the statuses of an order
/// </summary>
public enum eOrderStatus
    {
        Ordered,
        Shipped,
        Delivered
    }

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
    Order,
    Cart,
    Product
}

/// <summary>
/// enum for the options of the different actions on an order
/// </summary>
public enum eOrderOptions
{
    ReadAll = 1,
    Read,
    UpdateShipDate,
    UpdateDeliveryDate,
    track
}

/// <summary>
/// enum for the options of the different actions on a cart
/// </summary>
public enum eCartOptions
{
    AddProductToCart = 1,
    UpdateCart,
    Confirmation
}

/// <summary>
/// enum for the options of the different actions on a product
/// </summary>
public enum eProductOptions
{
    ReadAll = 1,
    Read,
    ReadForCatalog,
    Add,
    Delete,
    Update
}