namespace BlImplementation;
using BlApi;
using BO;
using System.Runtime.CompilerServices;

internal class BLOrder : IOrder
{
    private DalApi.IDal dalList { get; set; } = DalApi.Factory.Get();

    /// <summary>
    /// this function reads the list of orders
    /// </summary>
    /// <returns>the list of orders</returns>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.OrderForList> ReadOrderList()
    {
        try
        {
            lock (dalList)
            {
                var doOrders = dalList.Order.Read();
                List<BO.OrderForList> orderList = new();
                orderList = (from order in doOrders
                             select new BO.OrderForList
                             {
                                 ID = order.ID,
                                 CustomerName = order.CustomerName,
                                 Status = order.DeliveryDate != null ? (BO.eOrderStatus)2 : order.ShipDate != null ? (BO.eOrderStatus)1 : (BO.eOrderStatus)0,
                                 TotalPrice = dalList.OrderItem.Read(oi => oi.OrderId == order.ID).Sum(oi => oi.Price * oi.Amount),
                                 AmountOfItems = dalList.OrderItem.Read(oi => oi.OrderId == order.ID).Sum(oi => oi.Amount)
                             }).ToList();
                return orderList;
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }
    }


    /// <summary>
    /// this function reads the properties of a specific order
    /// </summary>
    /// <param name="orderId">the ID of the order wanted to be read</param>
    /// <returns>the order with the given ID</returns>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order ReadOrderProperties(int orderId)
    {
        BO.Order order = new();
        try
        {
            lock (dalList)
            {
                DO.Order DoOrder = dalList.Order.ReadSingle(o => o.ID == orderId);
                IEnumerable<DO.OrderItem>? DoOrderItems = dalList.OrderItem.Read(oi => oi.OrderId == orderId);
                order.ID = orderId;
                order.CustomerName = DoOrder.CustomerName;
                order.CustomerEmail = DoOrder.CustomerEmail;
                order.CustomerAddress = DoOrder.CustomerAddress;
                order.OrderDate = DoOrder.OrderDate;
                order.ShipDate = DoOrder.ShipDate;
                order.DeliveryDate = DoOrder.DeliveryDate;
                if (DoOrder.DeliveryDate != null)
                    order.Status = BO.eOrderStatus.Delivered;
                else if (DoOrder.ShipDate != null)
                    order.Status = BO.eOrderStatus.Shipped;
                else
                    order.Status = 0;
                order.Items =
                    (from oi in DoOrderItems
                     select new BO.OrderItem
                     {
                         ID = oi.ID,
                         ProductID = oi.ProductId,
                         ProductName = dalList.Product.ReadSingle(p => p.ID == oi.ProductId).Name,
                         Amount = oi.Amount,
                         Price = oi.Price,
                         TotalPrice = oi.Amount * oi.Price
                     }).ToList();
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }

        return order;
    }

    /// <summary>
    /// updates order as sent 
    /// </summary>
    /// <param name="orderId">the ID of the order which they want to update as sent</param>
    /// <returns>the updated order</returns>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    /// <exception cref="BlNoNeedToUpdateException">exception for no need to update</exception>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateOrderSent(int orderId)
    {
        BO.Order order = new();
        try
        {
            lock (dalList)
            {
                DO.Order DoOrder = dalList.Order.ReadSingle(o => o.ID == orderId);
                if (DoOrder.ID == 0)
                    throw new BlEntityNotFoundEx("order");
                if (DoOrder.ShipDate != null)
                    throw new BlNoNeedToUpdateException();
                DoOrder.ShipDate = DateTime.Now;
                dalList.Order.Update(DoOrder);
                order.ID = DoOrder.ID;
                order.CustomerName = DoOrder.CustomerName;
                order.CustomerEmail = DoOrder.CustomerEmail;
                order.CustomerAddress = DoOrder.CustomerAddress;
                order.Status = (BO.eOrderStatus)1;
                order.OrderDate = DoOrder.OrderDate;
                order.ShipDate = DateTime.Now;
                order.DeliveryDate = null;
                var DoOrderItems = dalList.OrderItem.Read(oi => oi.OrderId == orderId);
                order.Items = (from oi in DoOrderItems
                               select new OrderItem
                               {
                                   ID = oi.ID,
                                   ProductID = oi.ProductId,
                                   ProductName = dalList.Product.ReadSingle(p => p.ID == oi.ProductId).Name,
                                   Amount = oi.Amount,
                                   Price = oi.Price,
                                   TotalPrice = oi.Amount * oi.Price
                               }).ToList();
                order.TotalPrice = order.Items.Sum(oi => oi.TotalPrice);
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }
        return order;
    }


    /// <summary>
    /// updates order as delivered
    /// </summary>
    /// <param name="orderId">the ID of the order which they want to update as delivered</param>
    /// <returns>the updated order</returns>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    /// <exception cref="BlDateSeqException">exception for updating dates in wrong sequence</exception>
    /// <exception cref="BlNoNeedToUpdateException">exception for no need to update</exception>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateOrderDelivery(int orderId)
    {
        BO.Order order = new();
        try
        {
            lock (dalList)
            {
                DO.Order DoOrder = dalList.Order.ReadSingle(o => o.ID == orderId);
                if (DoOrder.ID == 0)
                    throw new BlEntityNotFoundEx("order");
                if (DoOrder.ShipDate == null)
                    throw new BlDateSeqException();
                if (DoOrder.DeliveryDate != null)
                    throw new BlNoNeedToUpdateException();
                DoOrder.DeliveryDate = DateTime.Now;
                dalList.Order.Update(DoOrder);
                order.ID = DoOrder.ID;
                order.CustomerName = DoOrder.CustomerName;
                order.CustomerEmail = DoOrder.CustomerEmail;
                order.CustomerAddress = DoOrder.CustomerAddress;
                order.OrderDate = DoOrder.OrderDate;
                order.ShipDate = DoOrder.ShipDate;
                order.DeliveryDate = DateTime.Now;
                order.Status = (BO.eOrderStatus)2;
                var DoOrderItems = dalList.OrderItem.Read(oi => oi.OrderId == orderId);
                order.Items = (from oi in DoOrderItems
                               select new OrderItem
                               {
                                   ID = oi.ID,
                                   ProductID = oi.ProductId,
                                   ProductName = dalList.Product.ReadSingle(p => p.ID == oi.ProductId).Name,
                                   Amount = oi.Amount,
                                   Price = oi.Price,
                                   TotalPrice = oi.Amount * oi.Price
                               }).ToList();
                order.TotalPrice = order.Items.Sum(oi => oi.TotalPrice);
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }
        return order;
    }

    /// <summary>
    /// this function tracks an order
    /// </summary>
    /// <param name="orderId">the ID of the order which they want to track</param>
    /// <returns>an order track</returns>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.OrderTracking TrackOrder(int orderId)
    {
        try
        {
            lock (dalList)
            {
                DO.Order order = dalList.Order.ReadSingle(o => o.ID == orderId);
                if (order.ID == 0)
                    throw new BlEntityNotFoundEx("order");
                BO.OrderTracking orderTracking = new();
                orderTracking.ID = order.ID;
                orderTracking.TrackList?.Add(new Tuple<DateTime?, eOrderStatus?>(order.OrderDate, BO.eOrderStatus.Ordered));
                orderTracking.Status = BO.eOrderStatus.Ordered;
                if (order.ShipDate != null)
                {
                    orderTracking.TrackList?.Add(new Tuple<DateTime?, eOrderStatus?>(order.ShipDate, BO.eOrderStatus.Shipped));
                    orderTracking.Status = eOrderStatus.Shipped;
                    if (order.DeliveryDate != null)
                    {
                        orderTracking.TrackList?.Add(new Tuple<DateTime?, eOrderStatus?>(order.DeliveryDate, BO.eOrderStatus.Delivered));
                        orderTracking.Status = BO.eOrderStatus.Delivered;
                    }
                }
                return orderTracking;
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order AddAmount(int orderId, int productId, int? addOrSubstract = null)
    {
        try
        {
            lock (dalList)
            {
                DO.OrderItem oi;
                if (addOrSubstract != null)
                {
                    oi = dalList.OrderItem.ReadSingle(o => o.OrderId == orderId && o.ProductId == productId);
                    if (addOrSubstract == 1)
                    {
                        oi.Amount++;
                        dalList.OrderItem.Update(oi);
                    }
                    else if (addOrSubstract == -1)
                    {
                        oi.Amount--;
                        dalList.OrderItem.Update(oi);
                    }
                    else if (addOrSubstract == 0)
                        dalList.OrderItem.Delete(oi.ID);
                    else
                        throw new BlinvalidRequest();
                }
                else
                {
                    oi = dalList.OrderItem.ReadSingle(oi => oi.OrderId == orderId && oi.ProductId == productId);
                    if (oi.ID != 0)
                        AddAmount(orderId, productId, 1);
                    else
                    {
                        oi = new();
                        oi.OrderId = orderId;
                        oi.ProductId = productId;
                        oi.Price = dalList.Product.ReadSingle(p => p.ID == productId).Price;
                        oi.Amount = 1;
                        dalList.OrderItem.Create(oi);
                    }
                }
            }
        }
        catch (BlApi.BlinvalidRequest ex)
        {
            throw new Exception();
        }
        catch (Exception ex)
        {
            throw new Exception();
        }
        return ReadOrderProperties(orderId);
    }

    /// <summary>
    /// chooses next order for the simulator to deal with
    /// </summary>
    /// <returns>ID of the next order to deal with</returns>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? ChooseOrder()
    {
        DateTime minDate = DateTime.Now;
        int? orderId = null;
        List<OrderForList>? orderList = ReadOrderList().ToList();
        orderList?.ForEach(o =>
        {
            switch (o.Status)
            {
                case eOrderStatus.Ordered:
                    if (ReadOrderProperties(o.ID).OrderDate < minDate)
                    {
                        orderId = o.ID;
                        minDate = (DateTime)ReadOrderProperties(o.ID).OrderDate;
                    }
                    break;
                case eOrderStatus.Shipped:
                    if (ReadOrderProperties(o.ID).ShipDate < minDate)
                    {
                        orderId = o.ID;
                        minDate = (DateTime)ReadOrderProperties(o.ID).ShipDate;
                    }
                    break;
                default:
                    break;
            }
        });
        return orderId;
    }
}