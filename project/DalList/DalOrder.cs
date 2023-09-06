using DO;
using DalApi;
using System.Runtime.CompilerServices;

namespace Dal;
/// <summary>
/// order functions
/// </summary>
internal class DalOrder:IOrder
{

    //adding an order
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Create(Order order)
    {
        order.ID = DataSource.Config.OrderId;
        DataSource.orderList.Add(order);
        return order.ID;    
    }


    //reading all orders
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order> Read(Func<Order,bool> func)
    {
        List<DO.Order> orders = new(DataSource.orderList);
        return func == null ? orders : orders.Where(func).ToList();
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order ReadSingle(Func<Order, bool> func)
    {
        List<Order> orders = (DataSource.orderList);
        //orders.AddRange();
        return orders.Where(func).FirstOrDefault();
    }


    //deleting an order
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Delete(int id)
    {
        bool deleted;
        deleted = DataSource.orderList.Remove(DataSource.orderList.Find(o => o.ID == id));
        if (!deleted)
            throw new EntityNotFoundException();
        return deleted;
    }


    //updating an order's deatails
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Update(Order order)
    {
        int idx = DataSource.orderList.FindIndex(o => o.ID == order.ID);
        if (idx > -1)
            DataSource.orderList[idx] = order;
        else
            throw new EntityNotFoundException();
        return true;
    }
}

