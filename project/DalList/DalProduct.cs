namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// product functions
/// </summary>
internal class DalProduct : IProduct
{
    //adding a product
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Create(Product product)
    {
        Random rand = new();
        bool idExists = false;
        int id;
        do
        {
            idExists = true;
            id = (int)rand.NextInt64(100000, 1000000);
            for (int j = 0; j < DataSource.productList.Count; j++)
                if (DataSource.productList[j].ID == id)
                    idExists = false;
        } while (!idExists);
        product.ID = id;
        DataSource.productList.Add(product);
        return id;
    }


    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Product> Read(Func<Product, bool>? func = null)
    {
        List<Product> products = new(DataSource.productList);
        return func == null ? products : products.Where(func).ToList();
    }

    public Product ReadSingle(Func<Product, bool> func)
    {
        List<Product> products = new(DataSource.productList);
        return products.Where(func).FirstOrDefault();
    }


    //deleting a product
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Delete(int id)
    {
        bool deleted = DataSource.productList.Remove(DataSource.productList.Find(p => p.ID == id));
        if (!deleted)
            throw new EntityNotFoundException();
        return deleted;
    }


    //updating a product's details
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Update(Product product)
    {
        DataSource.productList[DataSource.productList.FindIndex(p => p.ID == product.ID)] = product;

        int idx = DataSource.productList.FindIndex(p => p.ID == product.ID);
        if (idx > -1)
            DataSource.productList[idx] = product;
        else
            throw new EntityNotFoundException();
        return true;
    }
}
