namespace Dal;
using DO;
using DalApi;
using System.Runtime.CompilerServices;

/// <summary>
/// order-item functions
/// </summary>
internal class DalOrderItem : IOrderItem
{
    //adding an order-item
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Create(OrderItem oi)
    {
        oi.ID = DataSource.Config.OrderItemId;
        DataSource.orderItemList.Add(oi);
        return oi.ID;
    }


    //reading a specific order-item, according to its id
    [MethodImpl(MethodImplOptions.Synchronized)]
    public OrderItem Read(int id)
    {
        OrderItem oi = new();
        oi = DataSource.orderItemList.Find(oi => oi.ID == id);
        if (oi.ID == 0)
            throw new EntityNotFoundException();
        return oi;
    }


    //reading all order-items
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<OrderItem> Read(Func<OrderItem, bool>? func = null)
    {
        List<OrderItem> ois = new List<OrderItem>(DataSource.orderItemList);
        return func == null ? ois : ois.Where(func).ToList();
    }

    public OrderItem ReadSingle(Func<OrderItem, bool> func)
    {
        List<OrderItem> orderItems = new (DataSource.orderItemList);
        return orderItems.Where(func).FirstOrDefault();
    }


    //deleting an order-item
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Delete(int id)
    {
        bool deleted;
        deleted = DataSource.orderItemList.Remove(DataSource.orderItemList.Find(oi => oi.ID == id));
        if (!deleted)
            throw new EntityNotFoundException();
        return deleted;
    }


    //updating an order-item's details
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Update(OrderItem orderItem)
    {
        int idx = DataSource.orderItemList.FindIndex(oi => oi.ID == orderItem.ID);
        if (idx > -1)
            DataSource.orderItemList[idx] = orderItem;
        else
            throw new EntityNotFoundException();
        return true;
    }

}

