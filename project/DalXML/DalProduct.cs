namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

internal class DalProduct : IProduct
{
    /// <summary>
    /// creates a new product in xml
    /// </summary>
    /// <param name="product">product to create</param>
    /// <returns>ID of new product</returns>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Create(Product product)
    {
        Random rand = new();
        bool idExists = false;
        int id;
        List<Product> products = new List<Product>(Read());
        do
        {
            idExists = true;
            id = (int)rand.NextInt64(100000, 1000000);
            for (int j = 0; j < products.Count; j++)
                if (products[j].ID == id)
                    idExists = false;
        } while (!idExists);
        product.ID = id;
        List<Product> productList = Read().ToList();
        productList.Add(product);
        XmlSerializer ser = new XmlSerializer(typeof(List<Product>));
        StreamWriter w = new StreamWriter("../../xml/Product.xml");
        ser.Serialize(w, productList);
        w.Close();
        return id;
    }

    /// <summary>
    /// deletes a product from xml
    /// </summary>
    /// <param name="id">ID of product to be deleted</param>
    /// <returns>whethe product was deleted successfully</returns>
    /// <exception cref="EntityNotFoundException">exception for entity not found</exception>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Delete(int id)
    {
        List<Product> productList = Read().ToList();
        bool deleted = productList.Remove(productList.Find(p => p.ID == id));
        if (!deleted)
            throw new EntityNotFoundException("product");
        XmlSerializer ser = new XmlSerializer(typeof(List<Product>));
        StreamWriter w = new StreamWriter("../../xml/Product.xml");
        ser.Serialize(w, productList);
        w.Close();
        return deleted;
    }

    /// <summary>
    /// reads a list of products
    /// </summary>
    /// <param name="func">condition for the products to be read</param>
    /// <returns>list of products that apply for the condition in the func</returns>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Product> Read(Func<Product, bool>? func = null)
    {
        List<Product> productList = new List<Product>();
        StreamReader r = new("../../xml/Product.xml");
        XmlSerializer ser = new(typeof(List<Product>));
        productList = (List<Product>?)ser?.Deserialize(r);
        r.Close();
        return func == null ? productList : productList.Where(func).ToList();
    }

    /// <summary>
    /// reads a single product
    /// </summary>
    /// <param name="func">function that a specific product needs to apply</param>
    /// <returns>the first product that applies the condition</returns>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Product ReadSingle(Func<Product, bool> func)
    {
        return Read(func).FirstOrDefault();
    }

    /// <summary>
    /// updates product's details
    /// </summary>
    /// <param name="product">product with updates details</param>
    /// <returns>whether managed to update product successfully</returns>
    /// <exception cref="EntityNotFoundException">exception for entity not found</exception>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Update(Product product)
    {
        List<Product> productList = Read().ToList();
        productList[productList.FindIndex(p => p.ID == product.ID)] = product;
        int idx = productList.FindIndex(p => p.ID == product.ID);
        if (idx <= -1)
            throw new EntityNotFoundException("product");
        product.Name = product.Name == null ? productList[idx].Name:product.Name;
        product.InStock = product.InStock == null ? productList[idx].InStock : product.InStock;
        product.Price = product.Price == null ? productList[idx].Price : product.Price;
        product.Category = product.Category == null ? productList[idx].Category : product.Category;
        productList[idx] = product;
        XmlSerializer ser = new XmlSerializer(typeof(List<Product>));
        StreamWriter w = new StreamWriter("../../xml/Product.xml");
        ser.Serialize(w, productList);
        w.Close();
        return true;
    }
}