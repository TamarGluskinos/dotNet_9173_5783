using DO;
using DalApi;



IDal d = DalApi.Factory.Get();

//Order order= new Order();
//order.ID = 24;
//order.CustomerName = "ttami";
//order.CustomerAddress = null;
//order.CustomerEmail = "ssuri";
//order.OrderDate= null;
//order.ShipDate= DateTime.Today;
//order.DeliveryDate= null;
////d.Order.Create(order);

////d.Order.Delete(3);

//Console.WriteLine(d.Order.Read());

//d.Order.Update(order);

//Console.WriteLine("finish");
OrderItem orderItem = new OrderItem();
orderItem.ID = 8;
orderItem.OrderId = 9;
orderItem.ProductId = 999999;


//d.OrderItem.Create(orderItem);

//d.OrderItem.Delete(20);

//var X = d.OrderItem.Read();
//var A = d.OrderItem.ReadSingle(oi => oi.ID == 7);

//d.OrderItem.Update(orderItem);

//Product product = new Product();
//product.ID = 111111;
//product.Name = "new";
//product.Price = 5.5;
//product.InStock = 15;
//product.Category = eCategories.Breads;
//d.Product.Create(product);



//List<Product> aa= (List<Product>)d.Product.Read();

//Product p1 = d.Product.ReadSingle(e => e.ID == 456733);

//Product prod = new Product();
//prod.ID = 456733;
//prod.Name = "wednesday evening";
//prod.Price = 5.5;
//prod.InStock = 15;
//prod.Category = eCategories.Breads;
//d.Product.Update(prod);

//List<Product> bb = (List<Product>)d.Product.Read();

//Product p2 = d.Product.ReadSingle(e => e.ID == 456733);


//d.Product.Delete(480031);

//List<Product> cc = (List<Product>)d.Product.Read();

Console.WriteLine("success");