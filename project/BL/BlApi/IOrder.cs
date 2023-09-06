namespace BlApi;
/// <summary>
/// order interface
/// </summary>
public interface IOrder
{
    public IEnumerable<BO.OrderForList> ReadOrderList();
    public BO.Order ReadOrderProperties(int orderId);
    public BO.Order UpdateOrderSent(int orderId);
    public BO.Order UpdateOrderDelivery(int orderId);
    public BO.OrderTracking TrackOrder(int orderId);
    public BO.Order AddAmount(int productId, int orderId, int? Amount);
    public int? ChooseOrder();
}

