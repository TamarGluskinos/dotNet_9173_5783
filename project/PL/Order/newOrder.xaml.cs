using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BlImplementation;
using PL.Order;
namespace PL;

/// <summary>
/// Interaction logic for newOrder.xaml
/// </summary>
public partial class NewOrderWindow : Window
{
    /// <summary>
    /// bl instance
    /// </summary>
    private BlApi.IBL bl { get; set; }

    /// <summary>
    /// product window instance
    /// </summary>
    private ProductWindow? pw { get; set; }

    /// <summary>
    /// cart window instance
    /// </summary>
    private CartWindow? cw { get; set; }

    /// <summary>
    /// current PO cart instance
    /// </summary>
    public PO.Cart currentCart { get; set; }

    /// <summary>
    /// constructor for NewOrderWindow
    /// </summary>
    public NewOrderWindow()
    {
        InitializeComponent();
        bl = BlApi.Factory.Get();
        currentCart = new();
        ProductListView.ItemsSource = bl.Product.ReadProductItems(castPoCartToBoCart(currentCart));
        string[] IS = new string[Enum.GetNames(typeof(BO.eCategories)).Length + 1];
        string[] tmp = Enum.GetNames(typeof(BO.eCategories));
        IS[0] = "all categories";
        for (int i = 0; i < tmp.Length; i++)
        {
            IS[i + 1] = tmp[i];
        }
        cmbProductSelector.ItemsSource = IS;
        cmbProductSelector.SelectedIndex = 0;
    }

    /// <summary>
    /// casts from PO cart to BO cart
    /// </summary>
    /// <param name="c">the PO cart for casting</param>
    /// <returns>BO cart</returns>
    private BO.Cart castPoCartToBoCart(PO.Cart c)
    {
        BO.Cart cart = new BO.Cart();
        cart.CustomerName = c.CustomerName;
        cart.CustomerEmail = c.CustomerEmail;
        cart.CustomerAddress = c.CustomerAddress;
        cart.Items = c.Items;
        cart.Price = c.Price;
        return cart;
    }

    /// <summary>
    /// refreshes products on view
    /// </summary>
    public void ProductItemRefresh()
    {
        ProductListView.ItemsSource = bl.Product.ReadProductItems(castPoCartToBoCart(currentCart));
        if (cw != null)
            cw.refreshItems();
    }

    /// <summary>
    /// clears cart
    /// </summary>
    public void clearCart()
    {
        currentCart = new();
        ProductListView.ItemsSource = bl.Product.ReadProductItems(castPoCartToBoCart(currentCart));
    }

    /// <summary>
    /// selection of category changed- shows the products according to the new category
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if ((string)cmbProductSelector.SelectedItem != "all categories")
                ProductListView.ItemsSource = bl.Product.ReadProductItems(castPoCartToBoCart(currentCart),p => p.Category == (BO.eCategories)Enum.Parse(typeof(BO.eCategories), cmbProductSelector.SelectedItem.ToString()));
            else
                ProductListView.ItemsSource = bl.Product.ReadProductItems(castPoCartToBoCart(currentCart));
        }
        catch (BlApi.BlEntityNotFoundException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// shows a specific product
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        pw = new(bl, null, (ProductListView.SelectedItem as BO.ProductItem)?.ID, false, currentCart);
        pw.Owner = this;
        pw.Show();
    }

    /// <summary>
    /// shows current cart
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event argumentsf</param>
    private void btnViewCart_Click(object sender, RoutedEventArgs e)
    {
        cw = new(bl, currentCart);
        cw.Owner = this;
        cw.Show();
    }

}

