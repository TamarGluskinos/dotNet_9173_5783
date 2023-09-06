namespace DalApi;

public interface ICrud<T>
{
    public int Create(T t);
    public IEnumerable<T> Read(Func<T, bool>? func = null);
    public T ReadSingle(Func<T, bool> func);
    public bool Update(T t);
    public bool Delete(int id);
}

