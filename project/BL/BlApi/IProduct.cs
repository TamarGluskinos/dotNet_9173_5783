namespace BlApi;
/// <summary>
/// product interface
/// </summary>

public interface IProduct
{
    public IEnumerable<BO.ProductForList> ReadProductsList();
    public IEnumerable<BO.ProductForList> ReadProductsByCategory(BO.eCategories category);
    public BO.ProductItem ReadProductProperties(int productId, BO.Cart cart);
    public IEnumerable<BO.ProductItem> ReadProductItems(BO.Cart cart, Func<BO.ProductItem, bool>? func = null);
    public BO.Product ReadProductProperties(int productId);
    public void AddProduct(BO.Product prod);
    public void DeleteProduct(int productId);
    public void UpdateProduct(BO.Product prod);
}

