namespace DalApi;
public interface IDal
{
    public static IDal? Instance { get; }
    public IOrder Order { get; }
    public IOrderItem OrderItem { get; }
    public IProduct Product { get; }

}
