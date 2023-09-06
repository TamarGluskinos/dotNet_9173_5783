using BO;
using BlApi;

eOptions choice;
BlImplementation.BL Bl = new();
Cart cart = new();

try
{
    do
    {
        Console.WriteLine("click 0 to exit, \n 1 for orders, \n 2 for cart \n and 3 for products");
        choice = (eOptions)Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case eOptions.Exit:
                break;
            case eOptions.Order:
                OrderSwitch();
                break;
            case eOptions.Cart:
                CartSwitch();
                break;
            case eOptions.Product:
                ProductSwitch();
                break;
            default:
                Console.WriteLine("invalid choice");
                break;

        }

    } while (choice != 0);
}
catch (BlInvalidChoiceException ex)
{
    Console.WriteLine(ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


/// <summary>
/// different actions on order
/// </summary>
void OrderSwitch()
{
    try
    {
        Order order = new();
        OrderTracking orderTracking = new();
        IEnumerable<OrderForList> orderList = new List<OrderForList>();
        Console.WriteLine("click 1 to read order list, \n 2 to read order details, \n 3 to update order sent \n  4 to update delivery sent\n and 5 to track an order");
        eOrderOptions innerChoice;
        innerChoice = (eOrderOptions)Convert.ToInt32(Console.ReadLine());
        int orderId;
        switch (innerChoice)
        {
            case eOrderOptions.ReadAll:
                orderList = Bl.Order.ReadOrderList();
                foreach (BO.OrderForList o in orderList)
                { Console.WriteLine(o); }
                break;
            case eOrderOptions.Read:
                Console.WriteLine("enter the id of the order you would like to see");
                if (!(int.TryParse(Console.ReadLine(), out orderId)))
                    throw new BlInvalidIntegerException();
                order = Bl.Order.ReadOrderProperties(orderId);
                Console.WriteLine(order);
                break;
            case eOrderOptions.UpdateShipDate:
                Console.WriteLine("enter the id of the order you would like to Update ship date");
                if (!(int.TryParse(Console.ReadLine(), out orderId)))
                    throw new BlInvalidIntegerException();
                order = Bl.Order.UpdateOrderSent(orderId);
                Console.WriteLine("the updated order:" + order);
                break;
            case eOrderOptions.UpdateDeliveryDate:
                Console.WriteLine("enter the id of the order you would like to Update delivery date");
                if (!(int.TryParse(Console.ReadLine(), out orderId)))
                    throw new BlInvalidIntegerException();
                order = Bl.Order.UpdateOrderDelivery(orderId);
                Console.WriteLine("the updated order:" + order);
                break;
            case eOrderOptions.track:
                Console.WriteLine("enter the id of the order you would like to track");
                if (!(int.TryParse(Console.ReadLine(), out orderId)))
                    throw new BlInvalidIntegerException();
                orderTracking = Bl.Order.TrackOrder(orderId);
                Console.WriteLine(orderTracking);
                break;
            default:
                throw new BlInvalidChoiceException();
        }
    }
    catch(BlEntityNotFoundEx ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlInvalidChoiceException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlEntityNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlInvalidIntegerException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlNoNeedToUpdateException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}


/// <summary>
/// different actions on cart
/// </summary>
void CartSwitch()
{
    try
    {
        eCartOptions innerChoice;
        Console.WriteLine("click 1 to add a product to the cart, \n 2 to update the amount of the product in stock \n and 3 to confirm ");
        innerChoice = (eCartOptions)Convert.ToInt32(Console.ReadLine());
        int productId, newAmount;
        string CustomerName, CustomerEmail, CustomerAddress;

        switch (innerChoice)
        {
            case eCartOptions.AddProductToCart:
                Console.WriteLine("enter the id of the product you would like to add");
                if (!(int.TryParse(Console.ReadLine(), out productId)))
                    throw new BlInvalidIntegerException();
                cart = Bl.Cart.AddProductToCart(cart, productId);
                break;
            case eCartOptions.UpdateCart:
                Console.WriteLine("enter the id of the product you would like to update its amount");
                if (!(int.TryParse(Console.ReadLine(), out productId)))
                    throw new BlInvalidIntegerException();
                Console.WriteLine("enter the new amount");
                if (!(int.TryParse(Console.ReadLine(), out newAmount)))
                    throw new BlInvalidIntegerException();
                cart = Bl.Cart.UpdateProductAmount(cart, productId, newAmount);
                break;
            case eCartOptions.Confirmation:
                Console.WriteLine("enter the customer's name");
                CustomerName = Console.ReadLine();
                Console.WriteLine("enter the customer's email");
                CustomerEmail = Console.ReadLine();
                Console.WriteLine("enter the customer's address");
                CustomerAddress = Console.ReadLine();
                Bl.Cart.Confirmation(cart);
                break;
            default:
                throw new BlInvalidChoiceException();
        }
    }
    catch(BlEntityNotFoundEx ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlEntityNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlInvalidIntegerException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlInvalidChoiceException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlOutOfStockException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlNullValueException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}


/// <summary>
/// different actions on product
/// </summary>
void ProductSwitch()
{
    try
    {
        IEnumerable<ProductForList> productsLists = new List<ProductForList>();
        ProductItem productItem = new();
        Product product = new();
        Cart cart = new();
        int productId;
        Console.WriteLine("click 1 to read product list, \n 2 to read a specific product, \n 3 to read a product from catalog, \n" +
            "4 to add a product \n  5 to delete a product \n and 6 to update a product");
        eProductOptions innerChoice;
        innerChoice = (eProductOptions)Convert.ToInt32(Console.ReadLine());
        switch (innerChoice)
        {
            case eProductOptions.ReadAll:
                productsLists = Bl.Product.ReadProductsList();
                foreach (BO.ProductForList p in productsLists)
                { Console.WriteLine(p); }
                break;
            case eProductOptions.Read:
                Console.WriteLine("enter the id of the product you would like to see");
                if (!(int.TryParse(Console.ReadLine(), out productId)))
                    throw new BlInvalidIntegerException();
                product = Bl.Product.ReadProductProperties(productId);
                Console.WriteLine(product);
                break;
            case eProductOptions.ReadForCatalog:
                Console.WriteLine("enter the id of the product you would like to see");
                if (!(int.TryParse(Console.ReadLine(), out productId)))
                    throw new BlInvalidIntegerException();
                productItem = Bl.Product.ReadProductProperties(productId, cart);
                Console.WriteLine(productItem); 
                break;
            case eProductOptions.Add:
                product = AddProduct();
                Bl.Product.AddProduct(product);
                break;
            case eProductOptions.Delete:
                Console.WriteLine("enter the id of the product you would like to delete");
                if (!(int.TryParse(Console.ReadLine(), out productId)))
                    throw new BlInvalidIntegerException();
                Bl.Product.DeleteProduct(productId);
                break;
            case eProductOptions.Update:
                Console.WriteLine("enter the id of the product item you would like to update");
                if (!(int.TryParse(Console.ReadLine(), out productId)))
                    throw new BlInvalidIntegerException();
                product = Bl.Product.ReadProductProperties(productId);
                Console.WriteLine(product);
                UpdateProduct(product);
                break;
            default:
                throw new BlInvalidChoiceException();
        }

    }
    catch (BlNegativeValueException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlInvalidChoiceException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlEntityNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlInvalidIntegerException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (BlNoNeedToUpdateException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}


/// <summary>
/// add product to store
/// </summary>
Product AddProduct()
{
    Product product = new();
    product.ID = 0;
    Console.WriteLine("Enter the product's name:");
    product.Name = Console.ReadLine();
    Console.WriteLine("enter 0 if it is a type of bread, \n 1 if it is a type of milk, \n 2 if it is a treat," +
        "3 if it is a fruit \n and 4 if it is a vegetable");
    product.Category = (eCategories)Convert.ToInt32(Console.ReadLine());
    if ((int)product.Category > Enum.GetNames(typeof(eCategories)).Length || (int)product.Category < 0)
    {
        throw new BlInvalidChoiceException();
    }
    Console.WriteLine("enter the product's price");
    product.Price = Convert.ToDouble(Console.ReadLine());
    Console.WriteLine("enter the amount in stock");
    int integer;
    if (!(int.TryParse(Console.ReadLine(), out integer)))
        throw new BlInvalidIntegerException();
    product.InStock = integer;
    return product;
}


/// <summary>
/// update product details
/// </summary>
void UpdateProduct(Product product)
{
    string val;
    Console.WriteLine("Enter the product's name:");
    val = Console.ReadLine();
    if (!string.IsNullOrEmpty(val))
        product.Name = val;
    Console.WriteLine("enter 0 if it is a type of bread,\n 1 if it is a type of milk,\n 2 if it is a treat," +
        "3 if it is a fruit \n and 4 if it is a vegetable");
    val = Console.ReadLine();
    if (!string.IsNullOrEmpty(val))
        product.Category = (eCategories)Convert.ToInt32(val);
    if ((int)product.Category > Enum.GetNames(typeof(eCategories)).Length || (int)product.Category < 0)
    {
        throw new BlInvalidChoiceException();
    }
    Console.WriteLine("enter the product's price");
    val = Console.ReadLine();
    if (!string.IsNullOrEmpty(val))
        product.Price = Convert.ToDouble(val);
    Console.WriteLine("enter the amount in stock");
    val = Console.ReadLine();
    if (!string.IsNullOrEmpty(val))
        product.InStock = Convert.ToInt32(val);
    Bl.Product.UpdateProduct(product);
}