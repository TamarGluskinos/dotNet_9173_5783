namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

internal class DalOrder : IOrder
{
    /// <summary>
    /// creates a new order in xml
    /// </summary>
    /// <param name="order">order to create</param>
    /// <returns>ID of order added</returns>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Create(Order order)
    {
        XElement? rootConfig = XDocument.Load(@"..\..\xml\config.xml").Root;
        XElement? id = rootConfig?.Element("orderId");
        int orderID = Convert.ToInt32(id?.Value);
        orderID++;
        id.Value = orderID.ToString();
        rootConfig?.Save("../../xml/config.xml");
        order.ID = orderID;
        XElement o = new("Order",
                        new XElement("ID", order.ID),
                        new XElement("CustomerName", order.CustomerName),
                        new XElement("CustomerEmail", order.CustomerEmail),
                        new XElement("CustomerAddress", order.CustomerAddress),
                        new XElement("OrderDate", order.OrderDate),
                        new XElement("ShipDate", order.ShipDate),
                        new XElement("DeliveryDate", order.DeliveryDate));
        XElement? root = XDocument.Load("../../xml/Order.xml").Root;
        root?.Add(o);
        root?.Save("../../xml/Order.xml");
        return orderID;
    }

    /// <summary>
    /// deletes order from xml
    /// </summary>
    /// <param name="id">ID of order to delete</param>
    /// <returns>whether there was such an order to delete and if it was deleted successfully</returns>
     
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Delete(int id)
    {
        XElement? root = XDocument.Load("../../xml/Order.xml").Root;
        XElement? order = root?.Elements("Order")?.Where(o => o.Element("ID")?.Value == id.ToString()).FirstOrDefault();
        if (order == null)
            return false;
        order?.Remove();
        root?.Save("../../xml/Order.xml");
        return true;
    }

    /// <summary>
    /// reads a list of orders
    /// </summary>
    /// <param name="func">condition for the orders to be read</param>
    /// <returns>list of orders that apply for the condition in the func</returns>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order> Read(Func<Order, bool>? func = null)
    {
        XElement? root = XDocument.Load("../../xml/Order.xml")?.Root;
        List<Order> orderList = (from o in root?.Elements("Order")
                                 select new Order
                                 {
                                     ID = Convert.ToInt32(o.Element("ID")?.Value),
                                     CustomerName = o.Element("CustomerName")?.Value,
                                     CustomerEmail = o.Element("CustomerEmail")?.Value,
                                     CustomerAddress = o.Element("CustomerAddress")?.Value,
                                     OrderDate = o.Element("OrderDate")?.Value != "" ? Convert.ToDateTime(o.Element("OrderDate")?.Value) : null,
                                     ShipDate = o.Element("ShipDate")?.Value != "" ? Convert.ToDateTime(o.Element("ShipDate")?.Value) : null,
                                     DeliveryDate = (o.Element("DeliveryDate")?.Value) != "" ? Convert.ToDateTime(o.Element("DeliveryDate")?.Value) : null,
                                 }).ToList();
        return func == null ? orderList : orderList.Where(func).ToList();
    }

    /// <summary>
    /// reads a single order
    /// </summary>
    /// <param name="func">function that a specific order needs to apply</param>
    /// <returns>the first order that applies the condition</returns>
     
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order ReadSingle(Func<Order, bool> func)
    {
        return Read(func).FirstOrDefault();
    }

    /// <summary>
    /// updates an order's details
    /// </summary>
    /// <param name="order">updated order details</param>
    /// <returns>whether managed to update</returns>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Update(Order order)
    {
        XElement? root = XDocument.Load("../../xml/Order.xml").Root;
        XElement? update = root?.Elements("Order")?.
            Where(o => o.Element("ID")?.Value == order.ID.ToString()).FirstOrDefault();
        if (update == null)
            return false; 
        XElement o = new("Order",
                        new XElement("ID", order.ID),
                        new XElement("CustomerName", order.CustomerName),
                        new XElement("CustomerEmail", order.CustomerEmail),
                        new XElement("CustomerAddress", order.CustomerAddress),
                        new XElement("OrderDate", order.OrderDate),
                        new XElement("ShipDate", order.ShipDate),
                        new XElement("DeliveryDate", order.DeliveryDate));
        update?.ReplaceWith(o);
        root?.Save("../../xml/Order.xml");
        return true;
    }
}
