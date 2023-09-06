namespace BlApi;
/// <summary>
/// cart interface
/// </summary>
public interface ICart
{
    public BO.Cart AddProductToCart(BO.Cart cart, int productId);
    public BO.Cart UpdateProductAmount(BO.Cart cart, int productId, int newAmount);
    public void Confirmation(BO.Cart cart);
}

