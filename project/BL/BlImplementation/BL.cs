using BlApi;
namespace BlImplementation;

/// <summary>
/// BL object
/// </summary>

sealed public class BL:IBL
{
    public ICart Cart => new BLCart();
    public IOrder Order => new BLOrder();
    public IProduct Product => new BLProduct();
}

