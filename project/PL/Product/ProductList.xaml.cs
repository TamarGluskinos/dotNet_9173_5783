using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL;

/// <summary>
/// Interaction logic for Window1.xaml
/// </summary>
public partial class ProductListWindow : Window
{
    /// <summary>
    /// BL instance
    /// </summary>
    private BlApi.IBL Bl { get; set; }

    /// <summary>
    /// ProductWindow instance
    /// </summary>
    private ProductWindow? pw { get; set; }

    /// <summary>
    /// order ID
    /// </summary>
    private int? orderId { get; set; }

    /// <summary>
    /// refresh product shown in list
    /// </summary>
    public void ProductListRefresh()
    {
        ProductListView.ItemsSource = Bl.Product.ReadProductsList();
    }

    /// <summary>
    /// obsevable product list
    /// </summary>
    public ObservableCollection<BO.ProductForList>? productList { get; set; }

    /// <summary>
    /// ProductListWindow constructor
    /// </summary>
    /// <param name="OrderId">order ID</param>
    public ProductListWindow(int? OrderId = null)
    {
        InitializeComponent();
        Bl = BlApi.Factory.Get();
        if (OrderId == null)
            btnAddProduct.Visibility = Visibility.Visible;
        else
            btnAddProduct.Visibility = Visibility.Hidden;
        ProductListView.ItemsSource = Bl.Product.ReadProductsList();
        string[] IS = new string[Enum.GetNames(typeof(BO.eCategories)).Length + 1];
        string[] tmp = Enum.GetNames(typeof(BO.eCategories));
        IS[0] = "all categories";
        for (int i = 0; i < tmp.Length; i++) { IS[i + 1] = tmp[i]; }
        cmbProductSelector.ItemsSource = IS;
        cmbProductSelector.SelectedIndex = 0;
        orderId = OrderId;
    }

    /// <summary>
    /// select products according to category or shows all products
    /// </summary>
    /// <param name="sender">cmbProductSelector</param>
    /// <param name="e">selection changed</param>
    private void cmbProductSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if ((string)cmbProductSelector.SelectedItem != "all categories")
                ProductListView.ItemsSource = Bl?.Product.ReadProductsByCategory((BO.eCategories)Enum.Parse(typeof(BO.eCategories), cmbProductSelector.SelectedItem.ToString()));
            else
                ProductListView.ItemsSource = Bl?.Product.ReadProductsList();
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
    /// move to ProductWindow to update a certain product
    /// </summary>
    /// <param name="sender">ProductListView</param>
    /// <param name="e">mouse double clicked</param>
    private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        ProductWindow pw;
        if (orderId == null)
        {
            pw = new(Bl, this, (ProductListView.SelectedItem as BO.ProductForList)?.ID);
            pw.Owner = this;
            pw.Show();
        }
        else
        {
            pw = new(Bl, null, (ProductListView.SelectedItem as BO.ProductForList)?.ID, false, null, orderId);
            pw.Show();
            this.Close();
        }
    }

    /// <summary>
    /// moves to ProductWindow to add a product
    /// </summary>
    /// <param name="sender">btnAddProduct</param>
    /// <param name="e">button clicked</param>
    private void btnAddProduct_Click(object sender, RoutedEventArgs e)
    {
        ProductWindow pw = new(Bl, this);
        pw.Owner = this;
        pw.Show();
    }

}