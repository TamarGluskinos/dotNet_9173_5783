namespace DO;
/// <summary>
/// defining order-item struct
/// </summary>

public struct OrderItem
{
    public int ID { get; set; }
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }

    //overriding the ToString function for printing the order-item's details
    public override string ToString() => $@" Order Item: product ID : {ProductId} , order Id : {OrderId} 	price : {Price} amount: {Amount}";

}

