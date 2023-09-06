using DalApi;
using DO;
namespace Dal;

sealed internal class DalXml :IDal
{
    private static Lazy<DalXml> instance = new Lazy<DalXml>(() => new DalXml());
    public static DalXml Instance { get => GetInstance(); }
    private DalXml() { }
    public static DalXml GetInstance()
    {
        lock (instance)
        {
            if (instance == null)
                instance = new Lazy<DalXml>(() => new DalXml());
            return instance.Value;
        }
    }
    public IOrder Order { get; } = new DalOrder();
   public IOrderItem OrderItem { get; } = new DalOrderItem();
    public IProduct Product { get; } = new DalProduct();
}