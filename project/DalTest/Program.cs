using DO;
using DalApi;

eOptions choice;
int innerChoice;
IDal dallist = DalApi.Factory.Get();

try
{
    //switch for user to decide what he wants to deal with: orders, orderitems or products (or exit the program)
    do
    {
        Console.WriteLine("click 0 to exit, \n 1 for orders, \n 2 for orderitems \n and 3 for products");
        choice = (eOptions)Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case eOptions.Exit:
                break;
            case eOptions.Orders:
                orderCrud();
                break;
            case eOptions.OrderItems:
                orderItemCrud();
                break;
            case eOptions.Products:
                productCrud();
                break;
            default:
                throw new InvalidChoiceException();
                //throw new Exception("invalid choice");

        }

    } while (choice != 0);

}
catch (Exception ex)
{
    Console.WriteLine(ex);
}


//different functions on products
void productCrud()
{
    Product product;
    int productId;
    Console.WriteLine("click 1 to add a product, \n 2 to see a specific product, \n 3 to see all products, \n 4 to change an product's details \n or 5 to delete an product");
    innerChoice = Convert.ToInt32(Console.ReadLine());
    try
    {
        switch (innerChoice)
        {
            case (int)eCrudOptions.Create:
                product = AddProduct();
                dallist.Product.Create(product);
                break;
            case (int)eCrudOptions.ReadSingle:
                Console.WriteLine("enter the id of the product you would like to see");
                if (!(int.TryParse(Console.ReadLine(), out productId)))
                    throw new InvalidIntegerException();
                product = dallist.Product.ReadSingle(p => p.ID == productId);
                //product = dallist.Product.Read(prod=> prod.ID==productId) as Product;
                Console.WriteLine(product);
                break;
            case (int)eCrudOptions.ReadAll:
                IEnumerable<Product> allProducts = dallist.Product.Read();
                foreach (Product prod in allProducts)
                { Console.WriteLine(prod); }
                break;
            case (int)eCrudOptions.Update:
                Console.WriteLine("enter the id of the product item you would like to update");
                if (!(int.TryParse(Console.ReadLine(), out productId)))
                    throw new InvalidIntegerException();
                product = dallist.Product.ReadSingle(p => p.ID == productId);
                Console.WriteLine(product);
                UpdateProduct(product);
                break;
            case (int)eCrudOptions.Delete:
                Console.WriteLine("enter the id of the product you would like to delete");
                if (!(int.TryParse(Console.ReadLine(), out productId)))
                    throw new InvalidIntegerException();
                if (!dallist.Product.Delete(productId))
                    Console.WriteLine("couldn't delete your product");
                break;
            default:
                throw new InvalidChoiceException();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

}

//different functions on orders
void orderCrud()
{
    Order order;
    int orderId;
    Console.WriteLine("click 1 to add an order, \n 2 to see a specific order, \n 3 to see all orders, \n 4 to change an order's details \n or 5 to delete an order");
    innerChoice = Convert.ToInt32(Console.ReadLine());
    try
    {
        switch (innerChoice)
        {
            case (int)eCrudOptions.Create:
                order = AddOrder();
                dallist.Order.Create(order);
                break;
            case (int)eCrudOptions.ReadSingle:
                Console.WriteLine("enter the id of the order you would like to see");
                if (!(int.TryParse(Console.ReadLine(), out orderId)))
                    throw new InvalidIntegerException();
                order = dallist.Order.ReadSingle(o => o.ID == orderId);
                Console.WriteLine(order);
                break;
            case (int)eCrudOptions.ReadAll:
                IEnumerable<Order> allOrders = dallist.Order.Read();
                foreach (Order ord in allOrders)
                { Console.WriteLine(ord); }
                break;
            case (int)eCrudOptions.Update:
                Console.WriteLine("enter the id of the order you would like to update");
                if (!(int.TryParse(Console.ReadLine(), out orderId)))
                    throw new InvalidIntegerException();
                order = dallist.Order.ReadSingle(o => o.ID == orderId);
                Console.WriteLine(order);
                UpdateOrder(order);
                break;
            case (int)eCrudOptions.Delete:
                Console.WriteLine("enter the id of the order you would like to delete");
                if (!(int.TryParse(Console.ReadLine(), out orderId)))
                    throw new InvalidIntegerException();
                if (!dallist.Order.Delete(orderId))
                    Console.WriteLine("couldn't delete your order");
                break;
            default:
                throw new InvalidChoiceException();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

}

//different functions on order-items
void orderItemCrud()
{
    OrderItem orderItem;
    int orderItemId;
    int orderId;
    Console.WriteLine("click 1 to add an orderItem, \n 2 to see a specific orderItem by his Id, " +
        "3 to see a specific orderItem by orderId and productId, \n 4 to see the orderItems of a specific order," +
        "5 to see all orderItems, \n 6 to change an orderItem's details \n or 7 to delete an orderItem");
    innerChoice = Convert.ToInt32(Console.ReadLine());
    try
    {
        switch (innerChoice)
        {
            case (int)eOrderItems.Create:
                orderItem = AddOrderItem();
                 OrderItem ordItem = dallist.OrderItem.ReadSingle(oi => oi.OrderId == orderItem.OrderId && oi.ProductId == orderItem.ProductId);
                if(ordItem.ID == 0)
                dallist.OrderItem.Create(orderItem);
                else
                {
                    ordItem.Amount += orderItem.Amount;
                    dallist.OrderItem.Update(ordItem);
                }

                break;
            case (int)eOrderItems.ReadById:
                Console.WriteLine("enter the id of the orderItem you would like to see");
                if (!(int.TryParse(Console.ReadLine(), out orderItemId)))
                    throw new InvalidIntegerException();
                orderItem = dallist.OrderItem.ReadSingle(oi => oi.ID == orderItemId);
                Console.WriteLine(orderItem.ID!=0 ? orderItem:"no such order item");
                break;
            case (int)eOrderItems.ReadByOrderProductIds:
                Console.WriteLine("enter the id of the order and the id of the product of the orderitem you would like to see");
                if (!(int.TryParse(Console.ReadLine(), out orderId)))
                    throw new InvalidIntegerException();
                int productId;
                if (!(int.TryParse(Console.ReadLine(), out productId)))
                    throw new InvalidIntegerException();
                orderItem = dallist.OrderItem.ReadSingle(oi => oi.ProductId == productId && oi.OrderId == orderId);
                //orderItem = dallist.OrderItem.Read(productId, orderId);
                Console.WriteLine(orderItem);
                break;
            case (int)eOrderItems.ReadByOrderId:
                Console.WriteLine("enter the id of the orderItem you would like to see");
                if (!(int.TryParse(Console.ReadLine(), out orderId)))
                    throw new InvalidIntegerException();
                IEnumerable<OrderItem> orderItems = dallist.OrderItem.Read(oi => oi.OrderId == orderId);
                foreach (OrderItem oi in orderItems)
                { Console.WriteLine(oi); }
                break;
            case (int)eOrderItems.ReadAll:
                IEnumerable<OrderItem> allOrderItem = dallist.OrderItem.Read();
                foreach (OrderItem oi in allOrderItem)
                { Console.WriteLine(oi); }
                break;
            case (int)eOrderItems.Update:
                Console.WriteLine("enter the id of the order item you would like to update");
                if (!int.TryParse(Console.ReadLine(), out orderItemId))
                    throw new InvalidIntegerException();
                orderItem = dallist.OrderItem.ReadSingle(oi => oi.ID == orderItemId);
                Console.WriteLine(orderItem);
                UpdateOrderItem(orderItem);
                break;
            case (int)eOrderItems.Delete:
                Console.WriteLine("enter the id of the order item you would like to delete");
                if (!(int.TryParse(Console.ReadLine(), out orderItemId)))
                    throw new InvalidIntegerException();
                if (!dallist.OrderItem.Delete(orderItemId))
                    Console.WriteLine("couldn't delete your order-item");
                break;
            default:
                throw new InvalidChoiceException();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

}

//gets the details of the product you want to add
Product AddProduct()
{
    Product product = new Product();
    product.ID = 0;
    Console.WriteLine("Enter the product's name:");
    product.Name = Console.ReadLine();
    Console.WriteLine("enter 0 if it is a type of bread, \n 1 if it is a type of milk, \n 2 if it is a treat," +
        "3 if it is a fruit \n and 4 if it is a vegetable");
    product.Category = (eCategories)Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("enter the product's price");
    product.Price = Convert.ToDouble(Console.ReadLine());
    Console.WriteLine("enter the amount in stock");
    int integer;
    if (!(int.TryParse(Console.ReadLine(), out integer)))
        throw new InvalidIntegerException();
    product.InStock = integer;
    return product;
}

//gets the updated details of the product and updates them
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
    Console.WriteLine("enter the product's price");
    val = Console.ReadLine();
    if (!string.IsNullOrEmpty(val))
        product.Price = Convert.ToDouble(val);
    Console.WriteLine("enter the amount in stock");
    val = Console.ReadLine();
    if (!string.IsNullOrEmpty(val))
        product.InStock = Convert.ToInt32(val);
    bool updated = dallist.Product.Update(product);
    if (!updated)
        Console.WriteLine("couldn't update your product");
}

//gets the details of the order you want to add
Order AddOrder()
{
    Order order = new();
    try
    {
        order.ID = 0;
        Console.WriteLine("Enter your name:");
        order.CustomerName = Console.ReadLine();
        Console.WriteLine("Enter your Email:");
        order.CustomerEmail = Console.ReadLine();
        Console.WriteLine("Enter your address:");
        order.CustomerAddress = Console.ReadLine();
        Console.WriteLine("Enter an order date: ");
        DateTime userDateTime;
        if (DateTime.TryParse(Console.ReadLine(), out userDateTime))
            order.OrderDate = userDateTime;
        else
            throw new InvalidDateTimeException();
        Console.WriteLine("Enter a ship date: ");
        if (DateTime.TryParse(Console.ReadLine(), out userDateTime))
            order.ShipDate = userDateTime;
        else
            throw new InvalidDateTimeException();
        if (DateTime.Compare((DateTime)order.OrderDate, (DateTime)order.ShipDate) > 0)
            throw new Exception("ship date can't be before order date");
        Console.WriteLine("Enter a delivery date: ");
        if (DateTime.TryParse(Console.ReadLine(), out userDateTime))
            order.DeliveryDate = userDateTime;
        else
            throw new InvalidDateTimeException();
        if (DateTime.Compare((DateTime)order.OrderDate, (DateTime)order.ShipDate) > 0)
            throw new Exception("delivery date can't be before shio date");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
    return order;
}

//gets the updated details of the order and updates them
void UpdateOrder(Order order)
{
    try
    {
        DateTime userDateTime;
        string val;
        Console.WriteLine("Enter your name:");
        val = Console.ReadLine();
        if (!string.IsNullOrEmpty(val))
            order.CustomerName = val;
        Console.WriteLine("Enter your Email:");
        val = Console.ReadLine();
        if (!string.IsNullOrEmpty(val))
            order.CustomerEmail = val;
        Console.WriteLine("Enter your address:");
        val = Console.ReadLine();
        if (!string.IsNullOrEmpty(val))
            order.CustomerAddress = val;
        Console.WriteLine("Enter an order date: ");
        val = Console.ReadLine();
        if (!string.IsNullOrEmpty(val))
            if (DateTime.TryParse(val, out userDateTime))
                order.OrderDate = userDateTime;
            else
                throw new InvalidDateTimeException();
        Console.WriteLine("Enter a ship date: ");
        val = Console.ReadLine();
        if (!string.IsNullOrEmpty(val))
            if (DateTime.TryParse(val, out userDateTime))
                order.ShipDate = userDateTime;
            else
                throw new InvalidDateTimeException();
        if (DateTime.Compare((DateTime)order.OrderDate, (DateTime)order.ShipDate) > 0)
            throw new Exception("ship date can't be before order date");
        Console.WriteLine("Enter a delivery date: ");
        val = Console.ReadLine();
        if (!string.IsNullOrEmpty(val))
            if (DateTime.TryParse(val, out userDateTime))
                order.DeliveryDate = userDateTime;
            else
                throw new InvalidDateTimeException();
        if (DateTime.Compare((DateTime)order.OrderDate, (DateTime)order.ShipDate) > 0)
            throw new Exception("delivery date can't be before shio date");
        bool updated = dallist.Order.Update(order);
        if (!updated)
            Console.WriteLine("couldn't update your order");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}

//gets the details of the order-item you want to add
OrderItem AddOrderItem()
{
    OrderItem orderItem = new();
    Product product = new();
    orderItem.ID = 0; ;
    Console.WriteLine("Enter product id:");
    if (!(int.TryParse(Console.ReadLine(), out int prodIdx)))
        throw new InvalidIntegerException();
    Product prod = dallist.Product.ReadSingle(p => p.ID == prodIdx);
    if (prod.ID == 0) 
            throw new Exception("no such product in our store");
    orderItem.ProductId = prodIdx;
    Console.WriteLine("Enter order ID:");
    int orderId;
    if (int.TryParse(Console.ReadLine(), out orderId))
        orderItem.OrderId = orderId;
    else
        throw new InvalidIntegerException();
    DO.Order ord = dallist.Order.ReadSingle(o => o.ID == orderId);
    if (ord.ID == 0)
        throw new Exception("no such order");
    orderItem.OrderId = orderId;
    Console.WriteLine("Enter amount:");
    int amount;
    if (int.TryParse(Console.ReadLine(), out amount))
        orderItem.Amount = amount;
    else
        throw new InvalidIntegerException();

    product = dallist.Product.ReadSingle(p => p.ID == orderItem.ProductId);
    if (product.InStock >= amount)
        orderItem.Amount = amount;
    else
        orderItem.Amount = product.InStock;
    product.InStock -= orderItem.Amount;
    dallist.Product.Update(product);
    orderItem.Price = product.Price;
    return orderItem;
}

//gets the updated details of the order-item and updates them
void UpdateOrderItem(OrderItem orderItem)
{
    Product product = new();
    string val;
    Console.WriteLine("Enter product id:");
    if (!(int.TryParse(Console.ReadLine(), out int prodIdx)))
        throw new InvalidIntegerException();
    Product prod = dallist.Product.ReadSingle(p => p.ID == prodIdx);
    if (prod.ID == 0)
        throw new Exception("no such product in our store");
    orderItem.ProductId = prodIdx;
    Console.WriteLine("Enter order ID:");
    int orderId;
    if (int.TryParse(Console.ReadLine(), out orderId))
        orderItem.OrderId = orderId;
    else
        throw new InvalidIntegerException();
    DO.Order ord = dallist.Order.ReadSingle(o => o.ID == orderId);
    if (ord.ID == 0)
        throw new Exception("no such order");
    orderItem.OrderId = orderId;
    Console.WriteLine("Enter amount:");
    //val = Console.ReadLine();
    //if (!string.IsNullOrEmpty(val))
    //{
        int amount;
        if (int.TryParse(Console.ReadLine(), out amount))
            orderItem.Amount = amount;
        else
            throw new InvalidIntegerException();

        product = dallist.Product.ReadSingle(p => p.ID == orderItem.ProductId);
        if (product.InStock >= amount)
            orderItem.Amount = amount;
        else
            orderItem.Amount = product.InStock;
        product.InStock -= orderItem.Amount;
        dallist.Product.Update(product);
        orderItem.Price = product.Price;
  //  }
    bool updated = dallist.OrderItem.Update(orderItem);
    if (!updated)
        Console.WriteLine("couldn't update your order-item");
}








