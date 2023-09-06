namespace Dal;
using DO;
using DalApi;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

/// <summary>
/// order-item functions
/// </summary>
internal class DalOrderItem : IOrderItem
{
    /// <summary>
    /// adding an order item
    /// </summary>
    /// <param name="oi">tho order item to add</param>
    /// <returns>the id of the new order item</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Create(OrderItem oi)
    {
        XElement? rootConfig = XDocument.Load(@"..\..\xml\config.xml").Root;
        XElement? id = rootConfig?.Element("orderItemId");
        int orderItemID = Convert.ToInt32(id?.Value);
        orderItemID++;
        id.Value = orderItemID.ToString();
        rootConfig?.Save("../../xml/config.xml");
        oi.ID = orderItemID;
        List<OrderItem> orderItemList = Read().ToList();
        orderItemList.Add(oi);
        XmlSerializer ser = new XmlSerializer(typeof(List<OrderItem>));
        StreamWriter w = new StreamWriter("../../xml/OrderItem.xml");
        ser.Serialize(w, orderItemList);
        w.Close();
        return orderItemID;
    }


    /// <summary>
    /// read list of order items 
    /// </summary>
    /// <param name="func">opthional function to sort the order-items with</param>
    /// <returns>the order-items list</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<OrderItem> Read(Func<OrderItem, bool>? func = null)
    {
        List<OrderItem> orderItemList = new List<OrderItem>();
        StreamReader r = new("../../xml/OrderItem.xml");
        XmlSerializer ser = new(typeof(List<OrderItem>));
        orderItemList = (List<OrderItem>)ser.Deserialize(r);
        r.Close();
        List<OrderItem> ret = func == null ? orderItemList : orderItemList.Where(func).ToList();
        if (ret.Count == 0)
            ret.Add(new OrderItem());
        return ret;
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
    public OrderItem ReadSingle(Func<OrderItem, bool> func)
    {
        return Read(func).First();
    }


    /// <summary>
    /// delete an order-item 
    /// </summary>
    /// <param name="id">the id of the order-item to delete</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Delete(int id)
    {
        List<OrderItem> orderItemList = Read().ToList();
        bool deleted = orderItemList.Remove(orderItemList.Find(p => p.ID == id));
        if (!deleted)
            throw new EntityNotFoundException("order item");
        XmlSerializer ser = new XmlSerializer(typeof(List<OrderItem>));
        StreamWriter w = new StreamWriter("../../xml/OrderItem.xml");
        ser.Serialize(w, orderItemList);
        w.Close();
        return deleted;
    }

    /// <summary>
    /// update an order-item 
    /// </summary>
    /// <param name="orderItem">the id of the order-item to delete</param>
    /// <returns>boolean value if succeess to update</returns>
    /// <exception cref="EntityNotFoundException">exaption if an entity not found</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Update(OrderItem orderItem)
    {
        List<OrderItem> orderItemList = Read().ToList();
        int idx = orderItemList.FindIndex(oi => oi.ID == orderItem.ID);
        if (idx <= -1)
            throw new EntityNotFoundException("order item");
        orderItem.OrderId = orderItem.OrderId == 0 ? orderItemList[idx].OrderId : orderItem.OrderId;
        orderItem.ProductId = orderItem.ProductId == 0 ? orderItemList[idx].ProductId : orderItem.ProductId;
        orderItem.Price = orderItem.Price == 0 ? orderItemList[idx].Price : orderItem.Price;
        orderItem.Amount = orderItem.Amount == 0 ? orderItemList[idx].Amount : orderItem.Amount;
        orderItemList[idx] = orderItem;
        XmlSerializer ser = new XmlSerializer(typeof(List<OrderItem>));
        StreamWriter w = new StreamWriter("../../xml/OrderItem.xml");
        ser.Serialize(w, orderItemList);
        w.Close();
        return true;
    }

}

