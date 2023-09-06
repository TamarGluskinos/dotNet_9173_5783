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
using BlApi;
using PL;
namespace PL.Order;

/// <summary>
/// Interaction logic for CartWindow.xaml
/// </summary>
public partial class CartWindow : Window
{
    /// <summary>
    /// bl instance
    /// </summary>
    private BlApi.IBL bl { get; set; }

    /// <summary>
    /// dependency property for the visible state of a button
    /// </summary>
    public string visiblestate
    {
        get { return (string)GetValue(visiblestateProperty); }
        set { SetValue(visiblestateProperty, value); }
    }
    public static readonly DependencyProperty visiblestateProperty =
        DependencyProperty.Register("visiblestate", typeof(string), typeof(CartWindow), new PropertyMetadata("Hidden"));

    /// <summary>
    /// the window of Cart
    /// </summary>
    /// <param name="m_bl">bl instance</param>
    /// <param name="m_cart">current PO cart</param>
    public CartWindow(BlApi.IBL m_bl, PO.Cart m_cart)
    {
        InitializeComponent();
        currentCart = m_cart;
        bl = m_bl;
        this.DataContext = this;
    }

    /// <summary>
    /// refreshes- reloads items of the cart
    /// </summary>
    public void refreshItems()
    {
        listViewOrderItems.Items.Refresh();
    }
    public PO.Cart currentCart { get; set; }

    /// <summary>
    /// casts from BO cart to PO cart
    /// </summary>
    /// <param name="c">the BO cart for casting</param>
    private void castBoCartToCurrentCart(BO.Cart c)
    {
        currentCart.CustomerName = c.CustomerName;
        currentCart.CustomerEmail = c.CustomerEmail;
        currentCart.CustomerAddress = c.CustomerAddress;
        currentCart.Items = c.Items;
        currentCart.Price = (c.Price*100)/100;
    }

    /// <summary>
    /// casts from PO cart to BO cart
    /// </summary>
    /// <param name="c">the PO cart for casting</param>
    /// <returns></returns>
    private BO.Cart castPoCartToBoCart(PO.Cart c)
    {
        BO.Cart cart = new BO.Cart();
        cart.CustomerName = c.CustomerName;
        cart.CustomerEmail = c.CustomerEmail;
        cart.CustomerAddress = c.CustomerAddress;
        cart.Items = c.Items;
        cart.Price = (c.Price*100)/100;
        return cart;
    }

    /// <summary>
    /// order confirmation
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void btnConfirmOrder_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (txtCustomerName.Text == "")
                throw new PlNullValueException("customer name");
            if (txtCustomerEmail.Text == "")
                throw new PlNullValueException("customer email");
            System.Net.Mail.MailAddress addr = new(txtCustomerEmail.Text);
            bool isValidEmail = (addr.Address == txtCustomerEmail.Text);
            if (!(isValidEmail))
                throw new PlInvalidIntegerException();
            if (txtCustomerAddress.Text == "")
                throw new PlNullValueException("customer address");
            bl.Cart.Confirmation(castPoCartToBoCart(currentCart));
            MessageBox.Show("the order was confirmed");
            currentCart = new PO.Cart();
            ((NewOrderWindow)this.Owner).clearCart();
            this.Close();
        }
        catch (PlNullValueException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// function that deals with the event of finishing an order when clicking a button
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void btnFinishOrder_Click(object sender, RoutedEventArgs e)
    {
        visiblestate = "Visible";
    }

    /// <summary>
    /// add one to the amount of an item in the cart
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void btnPlus_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var productId = ((Button)sender).Tag;
            BO.OrderItem? orderItem = currentCart.Items?.Where(i => i.ProductID.Equals(productId)).First();
            int amount = orderItem.Amount;
            int prodID = orderItem.ProductID;
            castBoCartToCurrentCart(bl.Cart.UpdateProductAmount(castPoCartToBoCart(currentCart), prodID, amount + 1));
            ((NewOrderWindow)this.Owner).ProductItemRefresh();
        }
        catch(BlEntityNotFoundEx ex)
        {
            MessageBox.Show(ex.Message);    
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// substract one from the amount of an item in the cart
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void btnMinus_Click(object sender, RoutedEventArgs e)
    {
        try { 
        int productId = (int)((Button)sender).Tag;
        BO.OrderItem? orderItem = currentCart.Items?.Where(i => i.ProductID.Equals(productId)).First();
        int amount = orderItem.Amount;
        int prodID = orderItem.ProductID;
        castBoCartToCurrentCart(bl.Cart.UpdateProductAmount(castPoCartToBoCart(currentCart), prodID, amount - 1));
        listViewOrderItems.Items.Refresh();
        ((NewOrderWindow)this.Owner).ProductItemRefresh();
        }
        catch (BlEntityNotFoundEx ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// deletes an item from the cart
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        try { 
        var productId = ((Button)sender).Tag;
        BO.OrderItem? orderItem = currentCart.Items?.Where(i => i.ProductID.Equals(productId)).First();
        int? amount = orderItem?.Amount;
        int prodID = orderItem.ProductID;
        castBoCartToCurrentCart(bl.Cart.UpdateProductAmount(castPoCartToBoCart(currentCart), prodID, 0));
        listViewOrderItems.Items.Refresh();
        ((NewOrderWindow)this.Owner).ProductItemRefresh();
        }
        catch (BlEntityNotFoundEx ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

}
