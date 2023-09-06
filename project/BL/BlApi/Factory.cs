namespace BlApi;
/// <summary>
/// Bl factory
/// </summary>
public static class Factory
{
    public static IBL Get() { return new BlImplementation.BL(); }
}
