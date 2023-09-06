using DalApi;
using DO;
namespace Dal;

sealed internal class DalList : IDal
{
    private static Lazy<DalList> instance = new Lazy<DalList>(() => new DalList());
    public static DalList Instance { get => GetInstance(); }
    private DalList(){ }
    public static DalList GetInstance()
    {
        lock (instance)
        {
            if (instance == null)
                instance = new Lazy<DalList>(() => new DalList());
            return instance.Value;
        }
    }
    public IOrder Order => new DalOrder();
    public IOrderItem OrderItem => new DalOrderItem();
    public IProduct Product => new DalProduct();
}

