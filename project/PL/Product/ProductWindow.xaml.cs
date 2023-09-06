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
/// Interaction logic for Product.xaml
/// </summary>
public partial class ProductWindow : Window
{
    /// <summary>
    /// BL instance
    /// </summary>
    private BlApi.IBL Bl { get; set; }

    /// <summary>
    /// product instance
    /// </summary>
    public BO.Product? product { get; set; }

    /// <summary>
    /// current cart instance
    /// </summary>
    private PO.Cart? currentCart { get; set; }

    /// <summary>
    /// boolean variable to know if it is updateable
    /// </summary>
    public bool isUpdate { get; set; }

    /// <summary>
    /// boolean variable to know if it is changeable
    /// </summary>
    private bool isChanges { get; set; }

    /// <summary>
    /// order ID
    /// </summary>
    private int? orderId { get; set; }

    /// <summary>
    /// dependency property for visible state of delete button
    /// </summary>
    public bool btnDeleteVisiblestate
    {
        get { return (bool)GetValue(btnDeleteVisiblestateProperty); }
        set { SetValue(btnDeleteVisiblestateProperty, value); }
    }
    public static readonly DependencyProperty btnDeleteVisiblestateProperty =
        DependencyProperty.Register("btnDeleteVisiblestate", typeof(bool), typeof(ProductWindow), new PropertyMetadata(true));

    /// <summary>
    /// dependency property for visible state of add-update button
    /// </summary>
    public bool btnAddUpdateVisiblestate
    {
        get { return (bool)GetValue(btnAddUpdateVisiblestateProperty); }
        set { SetValue(btnAddUpdateVisiblestateProperty, value); }
    }
    public static readonly DependencyProperty btnAddUpdateVisiblestateProperty =
        DependencyProperty.Register("btnAddUpdateVisiblestate", typeof(bool), typeof(ProductWindow), new PropertyMetadata(true));

    /// <summary>
    /// dependency property for visible state change visible state
    /// </summary>
    public string ChangeVisiblestate
    {
        get { return (string)GetValue(ChangeVisiblestateProperty); }
        set { SetValue(ChangeVisiblestateProperty, value); }
    }
    public static readonly DependencyProperty ChangeVisiblestateProperty =
        DependencyProperty.Register("ChangeVisiblestate", typeof(string), typeof(ProductWindow), new PropertyMetadata("Visible"));


    /// <summary>
    /// ProductWindow constructor
    /// </summary>
    /// <param name="m_bl">bl instance</param>
    /// <param name="ProductId">ID of the product the user wants to update (or 0 if he wants to add a product)</param>
    public ProductWindow(BlApi.IBL m_bl, ProductListWindow? m_plw = null, int? ProductId = null, bool IsChanges = true, PO.Cart? m_cart = null, int? OrderId = null)
    {
        InitializeComponent();
        Bl = m_bl;
        isChanges = IsChanges;
        cmbProductCategorySelector.ItemsSource = Enum.GetValues(typeof(BO.eCategories));
        Init(ProductId);
        currentCart = m_cart;
        this.DataContext = this;
        orderId = OrderId;
    }

    /// <summary>
    /// initializes the screen for the user
    /// </summary>
    /// <param name="productId">ID of the product the user wants to update (or 0 if he wants to add a product)</param>
    private void Init(int? productId)
    {
        if (productId == null)
        {
            isUpdate = false;
            product = new();
            btnDeleteVisiblestate = false;
        }
        else
        {
            isUpdate = true;
            btnAddOrUpdate.Content = "Update";
            product = Bl.Product.ReadProductProperties((int)productId);
            this.DataContext = product;
        }
        if (isChanges == false)
        {
            btnDeleteVisiblestate = false;
            btnAddUpdateVisiblestate = false;
            txtProductName.IsReadOnly = true;
            txtProductCategory.IsReadOnly = true;
            txtProductPrice.IsReadOnly = true;
            txtProductAmount.IsReadOnly = true;
        }
        else
        {
            ChangeVisiblestate = "Hidden";
        }
    }

    /// <summary>
    /// adds or updates a product
    /// </summary>
    /// <param name="sender">btnAddOrUpdate</param>
    /// <param name="e">button clicked</param>
    private void btnAddOrUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (txtProductName.Text == "")
                throw new PlNullValueException("name");
            else if (txtProductAmount.Text == "")
                throw new PlNullValueException("amount");
            else if (txtProductPrice.Text == "")
                throw new PlNullValueException("price");
            else if (cmbProductCategorySelector.SelectedItem == null)
                throw new PlNullValueException("category");
            product.Name = txtProductName.Text;
            product.Category = (BO.eCategories)cmbProductCategorySelector.SelectedItem;
            if (!(int.TryParse(txtProductAmount.Text, out int intAmount)))
                throw new PlInvalidIntegerException();
            product.InStock = intAmount;
            if (!(Double.TryParse(txtProductPrice.Text, out double doublePrice)))
                throw new PlInvalidDoubleException();
            product.Price = doublePrice;
            if (isUpdate)
            {
                Bl.Product.UpdateProduct(product);
            }
            else
            {
                product.ID = 0;
                Bl.Product.AddProduct(product);
            }
            ((ProductListWindow)this.Owner).ProductListRefresh();
            string message = isUpdate == true ? "the product was updated successfully" : "the product was added successfuly";
            MessageBox.Show(message);
            this.Close();
            ((ProductListWindow)this.Owner).ProductListRefresh();
        }
        catch (BlApi.BlExistingIdException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (BlApi.BlNegativeValueException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (BlApi.BlNullValueException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (BlApi.BlEntityNotFoundException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (PlNullValueException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (PlInvalidIntegerException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (PlInvalidDoubleException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// deletes product
    /// </summary>
    /// <param name="sender">btnDelete</param>
    /// <param name="e">button clicked</param>
    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show($"are you sure you want to delete the {product?.Name}", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            try
            {
                Bl.Product.DeleteProduct(product.ID);
                ((ProductListWindow)this.Owner).ProductListRefresh();
                this.Close();
            }
            catch (BlApi.BlUnsuccessfulDeleteException ex)
            {
                MessageBox.Show(ex.Message);
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
    /// casts from BO cart to PO cart
    /// </summary>
    /// <param name="c">casted PO cart</param>
    private void castBoCartToPoCart(BO.Cart? c)
    {
        currentCart.CustomerName = c.CustomerName;
        currentCart.CustomerEmail = c.CustomerEmail;
        currentCart.CustomerAddress = c.CustomerAddress;
        currentCart.Items = c.Items;
        currentCart.Price = (c.Price * 100) / 100;
    }

    /// <summary>
    /// casts from PO cart to BO cart
    /// </summary>
    /// <param name="c">PO cart</param>
    /// <returns>casted BO cart</returns>
    private BO.Cart castPoCartToBoCart(PO.Cart? c)
    {
        BO.Cart cart = new BO.Cart();
        cart.CustomerName = c?.CustomerName;
        cart.CustomerEmail = c?.CustomerEmail;
        cart.CustomerAddress = c?.CustomerAddress;
        cart.Items = c?.Items;
        cart.Price = c.Price * 100 / 100;
        return cart;
    }

    /// <summary>
    /// adds to cart
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void btnAddToCart_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (orderId != null)
            {
                Bl.Order.AddAmount((int)orderId, product.ID, null);
                this.Close();
                return;
            }
            castBoCartToPoCart(Bl.Cart.AddProductToCart(castPoCartToBoCart(currentCart), product.ID));
            ((NewOrderWindow)this.Owner).ProductItemRefresh();
            this.Close();
        }
        catch (BlApi.BlOutOfStockException ex)
        {
            MessageBox.Show(ex.Message);
            this.Close();
        }
        catch (BlApi.BlExistingIdException ex)
        {
            MessageBox.Show(ex.Message);
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            this.Close();
        }
    }
}
