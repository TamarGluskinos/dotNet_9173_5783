namespace BlImplementation;
using BlApi;
using System.Runtime.CompilerServices;

internal class BLProduct : IProduct
{
    private DalApi.IDal dalList { get; set; } = DalApi.Factory.Get();
    /// <summary>
    /// this function reads the list of the products
    /// </summary>
    /// <returns>list of the products</returns>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.ProductForList> ReadProductsList()
    {
        try
        {
            lock (dalList)
            {
                IEnumerable<DO.Product> dalProduct = dalList.Product.Read();
                List<BO.ProductForList> products = new();
                products = (from p in dalProduct
                            select new BO.ProductForList
                            {
                                ID = p.ID,
                                Name = p.Name,
                                Price = p.Price,
                                Category = (BO.eCategories?)p.Category
                            }).ToList();
                return products;
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }
    }
    /// <summary>
    /// this function read the list of the products from that category
    /// </summary>
    /// <param name="category">the Category that wanted to read it's products</param>
    /// <returns>list of product from that category</returns>
    /// <exception cref="BlEntityNotFoundException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.ProductForList> ReadProductsByCategory(BO.eCategories category)
    {
        try
        {
            lock (dalList)
            {
                IEnumerable<DO.Product> dalProduct = dalList.Product.Read(p => (BO.eCategories?)p.Category == category);
                List<BO.ProductForList> products = new();
                products = (from p in dalProduct
                            select new BO.ProductForList
                            {
                                ID = p.ID,
                                Name = p.Name,
                                Price = p.Price,
                                Category = (BO.eCategories?)p.Category
                            }).ToList();
                return products;
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }
    }


    /// <summary>
    /// this function reads the details of a specific product
    /// </summary>
    /// <param name="productId">the ID of the product wanted to be read</param>
    /// <returns>the product with the specific ID</returns>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Product ReadProductProperties(int productId)
    {
        BO.Product product = new();
        try
        {
            lock (dalList)
            {
                if (productId <= 0)
                    throw new BlEntityNotFoundEx("product");
                DO.Product prod = dalList.Product.ReadSingle(p => p.ID == productId);
                product.ID = prod.ID;
                product.Name = prod.Name;
                product.Price = prod.Price;
                product.Category = (BO.eCategories)prod.Category;
                product.InStock = prod.InStock;
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }
        return product;
    }

    /// <summary>
    /// this function returns how many of each item in the store are in the cart
    /// </summary>
    /// <param name="cart">the cart to be checked</param>
    /// <returns>list of products in store and their amount in the specific cart</returns>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.ProductItem ReadProductProperties(int productId, BO.Cart cart)
    {
        try
        {
            lock (dalList)
            {
                DO.Product DOProduct = dalList.Product.ReadSingle(p => p.ID == productId);
                BO.ProductItem product = new();
                product.ID = DOProduct.ID;
                product.Name = DOProduct.Name;
                product.Price = DOProduct.Price;
                product.Category = (BO.eCategories)DOProduct.Category;
                if (cart.Items?.Find(oi => oi.ProductID == productId) == null)
                {
                    product.Amount = 0;
                    product.IsInStock = DOProduct.InStock > 0;
                }
                else
                {
                    product.Amount = cart.Items.Find(oi => oi.ProductID == productId).Amount;
                    product.IsInStock = DOProduct.InStock >= product.Amount;
                }
                return product;
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.ProductItem> ReadProductItems(BO.Cart cart, Func<BO.ProductItem, bool>? func = null)
    {
        try
        {
            lock (dalList)
            {
                IEnumerable<DO.Product> dalProduct = dalList.Product.Read();
                List<BO.ProductItem> products = new();
                products = (from p in dalProduct
                            orderby p.Category
                            select new BO.ProductItem
                            {
                                ID = p.ID,
                                Name = p.Name,
                                Price = p.Price,
                                Category = (BO.eCategories?)p.Category,
                                Amount = cart.Items?.Find(oi => oi?.ProductID == p.ID) == null ? 0 : cart.Items.Find(oi => oi.ProductID == p.ID).Amount,
                                IsInStock = cart.Items?.Find(oi => oi?.ProductID == p.ID) == null ? (p.InStock > 0 ? true : false) : (p.InStock >= cart.Items.Find(oi => oi.ProductID == p.ID).Amount ? true : false)
                            }).ToList();
                if (func != null)
                    products = products.Where(func).ToList();

                return products;
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }
    }

    /// <summary>
    /// this function adds an item to data base
    /// </summary>
    /// <param name="prod">the product to be added</param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void AddProduct(BO.Product prod)
    {
        lock (dalList)
        {
            if (string.IsNullOrEmpty(prod.Name))
                throw new BlNullValueException();
            if (prod.Price <= 0)
                throw new BlNegativeValueException();
            if (prod.InStock < 0)
                throw new BlNegativeValueException();
            DO.Product DOProduct = new();
            DO.Product productWithExistingId = dalList.Product.ReadSingle(p => p.ID == DOProduct.ID);
            if (productWithExistingId.ID != 0)
                throw new BlExistingIdException("product");
            DOProduct.ID = prod.ID;
            DOProduct.Name = prod.Name;
            DOProduct.Price = prod.Price;
            DOProduct.Category = (DO.eCategories)prod.Category;
            DOProduct.InStock = prod.InStock;
            dalList.Product.Create(DOProduct);
        }
    }

    /// <summary>
    /// this function deleted a product from the data base
    /// </summary>
    /// 
    /// orderitems[0].ProductId == productId && dalList.Order.ReadSingle(o => o.ID == orderitems[0].OrderId).DeliveryDate
    /// 
    /// <param name="productId">the ID of the product to be deleted</param>
    /// <exception cref="BlUnsuccessfulDeleteException">exception for unsuccessful attempt to delete</exception>
    /// <exception cref="BlEntityNotFoundException">exception for entity not found</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteProduct(int productId)
    {
        try
        {
            lock (dalList)
            {
                IEnumerable<DO.OrderItem> orderitems = dalList.OrderItem.Read();
                List<DO.OrderItem> x = orderitems.Where(oi => oi.ProductId == productId &&
                 dalList.Order.ReadSingle(o => o.ID == oi.OrderId).DeliveryDate == null).ToList();
                if (x.Count > 0)
                    throw new BlUnsuccessfulDeleteException();
                bool deleted = dalList.Product.Delete(productId);
                if (!deleted)
                    throw new BlUnsuccessfulDeleteException();
            }
        }
        catch (DalApi.EntityNotFoundException ex)
        {
            throw new BlEntityNotFoundException(ex);
        }

    }

    /// <summary>
    /// updates details of product
    /// </summary>
    /// <param name="prod">the product for updating</param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UpdateProduct(BO.Product prod)
    {
        lock (dalList)
        {
            if (prod.ID <= 0)
                throw new BlEntityNotFoundEx("product");
            if (string.IsNullOrEmpty(prod.Name))
                throw new BlNullValueException();
            if (prod.Price <= 0)
                throw new BlNegativeValueException();
            if (prod.InStock < 0)
                throw new BlNegativeValueException();
            DO.Product DOProduct = new();
            DOProduct.ID = prod.ID;
            DOProduct.Name = prod.Name;
            DOProduct.Price = prod.Price;
            DOProduct.Category = (DO.eCategories)prod.Category;
            DOProduct.InStock = prod.InStock;
            dalList.Product.Update(DOProduct);
        }
    }
}
