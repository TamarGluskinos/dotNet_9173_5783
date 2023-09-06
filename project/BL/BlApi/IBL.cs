namespace BlApi;
/// <summary>
/// BL interface
/// </summary>

public interface IBL
{
    public ICart Cart { get; }
    public IOrder Order { get; }
    public IProduct Product { get; }
}

