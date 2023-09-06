namespace BlImplementation;
using BlApi;
using System.Runtime.CompilerServices;

public class BLCart : ICart
{
    private DalApi.IDal dalList { get; set; } = DalApi.Factory.Get();

    /// <summary>
    /// this function adds a product to a cart
    /// </summary>
    /// <param name="cart">the customer's cart</param>
    /// <param name="productId">the id of the product the customer wants to add to the customer's cart</param>
    /// <returns>the cart with the product added</returns>
    /// <exception cref="BlOutOfStockException">exception for item out of stock</exception>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Cart AddProductToCart(BO.Cart cart, int productId)
    {
        try
        {
            lock (dalList)
            {
                BO.OrderItem OiWithExistingId = cart.Items?.Where(i => i.ProductID == productId).FirstOrDefault();
                if (OiWithExistingId != null && OiWithExistingId.Amount > 0)
                    throw new BlApi.BlExistingIdException("order item");
                DO.Product prod = dalList.Product.ReadSingle(prod => prod.ID == productId);
                int productInStock = prod.InStock;
                double productPrice = prod.Price;
                BO.OrderItem? oi = new();
                if (cart.Items != null)
                    oi = cart.Items?.Find(oi => oi.ProductID == productId);
                if (productInStock > 0)
                {
                    if (oi != null && oi.ID != 0)
                    {
                        oi.Amount++;
                        oi.TotalPrice += productPrice;
                        cart.Price += productPrice;
                        return cart;
                    }
                    else
                    {
                        BO.OrderItem oItem = new();
                        oItem.ID = 0;
                        oItem.ProductID = productId;
                        oItem.ProductName = prod.Name;
                        oItem.Amount = 1;
                        oItem.Price = productPrice;
                        oItem.TotalPrice = productPrice;
                        if (cart.Items == null)
                            cart.Items = new List<BO.OrderItem>();
                        List<BO.OrderItem> localOI = cart.Items;
                        localOI.Add(oItem);
                        cart.Items = localOI;
                        cart.Price += productPrice;
                    }
                    return cart;
                }
                else
                    throw new BlOutOfStockException();
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }

    }

    /// <summary>
    /// this function updates the amount of the product in the cart
    /// </summary>
    /// <param name="cart">the customer's cart</param>
    /// <param name="productId">the ID of the product the customer wants to add</param>
    /// <param name="newAmount">the new amount of the item</param>
    /// <returns>updated cart</returns>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    /// <exception cref="BlOutOfStockException">eception for item out of stock</exception>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Cart UpdateProductAmount(BO.Cart cart, int productId, int newAmount)
    {
        try
        {
            lock (dalList)
            {
                int productInStock = dalList.Product.ReadSingle(prod => prod.ID == productId).InStock;
                double productPrice = dalList.Product.ReadSingle(prod => prod.ID == productId).Price;
                BO.OrderItem? oi = cart?.Items?.Find(oi => oi?.ProductID == productId);
                if (oi == null)
                    throw new BlEntityNotFoundEx("order item");
                if (newAmount > oi.Amount)
                {
                    //if (newAmount - oi.Amount > productInStock)
                    if (newAmount > productInStock)
                        throw new BlOutOfStockException();
                    cart.Price += (newAmount - oi.Amount) * productPrice;
                    oi.TotalPrice += (newAmount - oi.Amount) * productPrice;
                    oi.Amount = newAmount;
                }
                else if (newAmount == 0)
                {
                    cart.Price -= productPrice * oi.Amount;
                    cart.Items?.Remove(oi);
                }
                else
                {
                    cart.Price -= productPrice * (oi.Amount - newAmount);
                    oi.Amount = newAmount;
                    oi.TotalPrice = productPrice * newAmount;
                }

                return cart;
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }
    }

    /// <summary>
    /// this function confirms the cart details
    /// </summary>
    /// <param name="cart">the customer's cart</param>
    /// <param name="CustomerName">the customer's name</param>
    /// <param name="CustomerEmail">the customer's email</param>
    /// <param name="CustomerAddress">the customer's address</param>
    /// <exception cref="BlNullValueException">exception for invalid null value</exception>
    /// <exception cref="BlInvalidEmailException">exception for invalid email</exception>
    /// <exception cref="BlNegativeValueException">exception for invalid negative value</exception>
    /// <exception cref="BlOutOfStockException">exception for item out of stock</exception>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Confirmation(BO.Cart cart)
    {
        try
        {
            lock (dalList)
            {
                System.Net.Mail.MailAddress addr = new(cart.CustomerEmail);
                bool isValidEmail = (addr.Address == cart.CustomerEmail);
                if (cart.CustomerName == "" || cart.CustomerAddress == "")
                {
                    throw new BlNullValueException();
                }
                if (!isValidEmail)
                {
                    throw new BlInvalidEmailException();
                }
                int productInStock;
                cart.Items?.ForEach(oi =>
                {
                    productInStock = dalList.Product.ReadSingle(p => p.ID == oi?.ProductID).InStock;
                    if (oi?.Amount < 0)
                    {
                        throw new BlNegativeValueException();
                    }
                    if (productInStock < oi?.Amount)
                    {
                        throw new BlOutOfStockException();
                    }

                    List<DO.Product> prodList = dalList?.Product?.Read() as List<DO.Product>;
                    bool idExists = (prodList.Find(p => p.ID == oi?.ProductID)).ID != 0;
                    if (!idExists)
                        throw new BlEntityNotFoundEx("product");
                });

                DO.Order DoOrder = new();
                DoOrder.OrderDate = DateTime.Now;
                DoOrder.ShipDate = null;
                DoOrder.DeliveryDate = null;
                DoOrder.CustomerName = cart.CustomerName;
                DoOrder.CustomerEmail = cart.CustomerEmail;
                DoOrder.CustomerAddress = cart.CustomerAddress;
                DoOrder.ID = 0;
                int orderId = dalList.Order.Create(DoOrder);

                DO.OrderItem DoOrderItem = new();
                cart.Items?.ForEach(oi =>
                {
                    DoOrderItem.ID = oi.ID;
                    DoOrderItem.ProductId = oi.ProductID;
                    DoOrderItem.OrderId = orderId;// DoOrder.ID;
                    DoOrderItem.Amount = oi.Amount;
                    DoOrderItem.Price = oi.TotalPrice;
                    dalList.OrderItem.Create(DoOrderItem);
                    DO.Product prod = dalList.Product.ReadSingle(p => p.ID == DoOrderItem.ProductId);
                    prod.InStock -= oi.Amount;
                    dalList.Product.Update(prod);
                });
                Console.WriteLine("confirmed");
            }
        }
        catch (DalApi.EntityNotFoundException)
        {
            throw new BlNegativeValueException();
        }
    }
}