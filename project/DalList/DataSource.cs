
namespace Dal;
using DO;

/// <summary>
/// handling the store's data source
/// </summary>
public static class DataSource

{
    internal static readonly Random Randomize = new();

    public static List<Product> productList = new();
    public static List<Order> orderList = new();
    public static List<OrderItem> orderItemList = new();


    static DataSource() { s_Initialize(); }

    //initializing our info
    private static void s_Initialize()
    {
        InitProductArray();
        InitOrderArray();
        InitOrderItemArray();
    }

    //handing the ids and indexes of our structs
    public static class Config
    {
        private static int orderItemId = 1;
        public static int OrderItemId { get { return orderItemId++; } }
        private static int orderId = 1;
        public static int OrderId { get { return orderId++; } }

    }


    //initializes the products info
    private static void InitProductArray()
    {
        bool exists;
        int id;

        (string, eCategories)[] productNames = new (string, eCategories)[10]
           {("white bread", eCategories.Breads ), ("whole wheat rolls", eCategories.Breads), ("milk", eCategories.Milky),
            ("cream cheese", eCategories.Milky), ("jellies", eCategories.Treats), ("toffies", eCategories.Treats),
            ("apple", eCategories.Fruit), ("orange", eCategories.Fruit), ("cucumber", eCategories.Veg), ("tomato", eCategories.Veg)};
        for (int i = 0; i < 10; i++)
        {
            Product prod = new();
            do
            {
                exists = true;
                id = (int)Randomize.NextInt64(100000, 1000000);
                for (int j = 0; j < productList.Count; j++)
                {
                    if (productList[j].ID == id)
                        exists = false;
                }
            } while (!exists);
            prod.ID = id;
            (prod.Name, prod.Category) = productNames[i];
            double price = (double)Randomize.NextDouble() * 10 + 5; //5-15
            price = Math.Floor(price * 100) / 100;
            prod.Price = price;
            int inStock = (int)Randomize.NextInt64(0, 100);
            prod.InStock = inStock;
            productList.Add(prod);
        }
    }


    //initializes the order-items info
    private static void InitOrderItemArray()
    {
        Product product = new();
        for (int i = 0; i < 10 /* orderItemList.Count*/; i++) //goes over all orders and enters items to each order
        {
            int num = (int)Randomize.NextInt64(2, 5); //1-4
            for (int j = 0; j < num; j++)
            {
                OrderItem oi = new();
                oi.ID = Config.OrderItemId;
                oi.OrderId = orderList[i].ID;
                int idx = (int)Randomize.NextInt64(0, productList.Count);
                oi.ProductId = productList[idx].ID;
                int amount = (int)Randomize.NextInt64(1, 10); //1-9
                if (productList[idx].InStock >= amount)
                    oi.Amount = amount;
                else
                    oi.Amount = productList[idx].InStock;
                product = productList[idx];
                product.InStock -= oi.Amount;
                productList[idx] = product;
                oi.Price = productList[idx].Price;
                orderItemList.Add(oi);
            }

        }
    }


    //initializes the order info
    private static void InitOrderArray()
    {
        string[] customerNames = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t" };
        string[] customerEmails = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t" };
        string[] customerAddresses = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t" };

        for (int i = 0; i < 20; i++)
        {
            Order order = new();
            order.ID = Config.OrderId;
            order.CustomerName = customerNames[i];
            order.CustomerEmail = customerEmails[i];
            order.CustomerAddress = customerAddresses[i];

            //randomizes a date from 01/01/2010
            Random ran = new();
            DateTime start = new DateTime(2010, 1, 1);
            int range = (DateTime.Today - start).Days;
            order.OrderDate = start.AddDays(ran.Next(range));


            int dateShipExsist = (int)Randomize.NextInt64(0, 5);
            if (dateShipExsist > 0)
            {
                TimeSpan spanOrderShip = TimeSpan.FromDays(5);
                order.ShipDate = order.OrderDate + spanOrderShip;
                int dateDeliveryExsist = (int)Randomize.NextInt64(0, 5);
                if (dateDeliveryExsist > 0)
                {
                    TimeSpan spanShipDelivery = TimeSpan.FromDays(30);
                    order.DeliveryDate = order.ShipDate + spanShipDelivery;
                }
                else
                    order.DeliveryDate = null;
            }
            else
            {
                order.ShipDate = null;
                order.DeliveryDate = null;
            }

            orderList.Add(order);
        }
    }
}


