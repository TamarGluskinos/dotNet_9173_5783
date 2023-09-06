using BlApi;
using BlImplementation;
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

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        /// <summary>
        /// BL instance
        /// </summary>
        private BlApi.IBL Bl { get; set; }

        /// <summary>
        /// BO order instance
        /// </summary>
        private BO.Order? order { get; set; }

        /// <summary>
        /// boolean variable to know whether it is changeable
        /// </summary>
        private bool isChange { get; set; }

        /// <summary>
        /// order ID
        /// </summary>
        public int? OrdId { get; set; } = null;

        /// <summary>
        /// observable order list
        /// </summary>
        private ObservableCollection<BO.OrderForList>? orderList { get; set; }

        /// <summary>
        /// observable order items
        /// </summary>
        private ObservableCollection<BO.OrderItem>? orderItems { get; set; }

        /// <summary>
        /// OrderWindow cunstructor
        /// </summary>
        /// <param name="m_bl">BL instance</param>
        /// <param name="orderId">order ID</param>
        /// <param name="IsChange">value of boolean IsChange variable</param>
        /// <param name="m_orderList">order-list</param>
        public OrderWindow(BlApi.IBL m_bl, int? orderId = null, bool IsChange = true, ObservableCollection<BO.OrderForList>? m_orderList = null)
        {
            InitializeComponent();
            OrdId = orderId;
            Bl = m_bl;
            orderList = m_orderList;
            isChange = IsChange;
            Init(orderId);
        }

        /// <summary>
        /// initializer
        /// </summary>
        /// <param name="orderId">order ID</param>
        private void Init(int? orderId)
        {
            if (orderId == null)
            {
                order = new();
                return;
            }
            order = Bl.Order.ReadOrderProperties((int)orderId);
            this.DataContext = order;
            orderItems = new ObservableCollection<BO.OrderItem>(order.Items);
            listViewOrderItems.ItemsSource = orderItems;
            if (order.Status == BO.eOrderStatus.Delivered)
            {
                btnUpdateStatus.Visibility = Visibility.Hidden;
            }
            else if (order.Status == BO.eOrderStatus.Shipped)
                btnUpdateStatus.Content = "update product delivered";
            else if (order.Status == BO.eOrderStatus.Ordered)
                btnUpdateStatus.Content = "update product shipped";
            if (isChange == false)
            {
                btnUpdateStatus.Visibility = Visibility.Hidden;
                btnAddProduct.Visibility = Visibility.Hidden;
                gvcProductName.Width = 85;
                gvcPlus.Width = 0;
                gvcMinus.Width = 0;
                gvcX.Width = 0;
                txtOrderCustomerAddress.IsReadOnly = true;
                txtOrderCustomerEmail.IsReadOnly = true;
                txtOrderCustomerName.IsReadOnly = true;
                txtOrderId.IsReadOnly = true;
                txtOrderStatus.IsReadOnly = true;
            }
        }

        /// <summary>
        /// updates status of order
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event arguments</param>
        private void btnUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (order?.Status == BO.eOrderStatus.Delivered)
                    return;
                else if (order?.Status == BO.eOrderStatus.Shipped)
                {
                    Bl.Order.UpdateOrderDelivery(Convert.ToInt32(OrdId));
                    this.Close();
                }
                else
                {
                    Bl.Order.UpdateOrderSent(Convert.ToInt32(OrdId));
                    this.Close();
                }
                orderList?.Clear();
                Bl.Order.ReadOrderList().ToList().ForEach(item => orderList?.Add(item));
            }
            catch(BlEntityNotFoundEx ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);    
            }
        }

        /// <summary>
        /// add amount of an item to an order
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event arguments</param>
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var productId = ((Button)sender).Tag;
                order = Bl.Order.AddAmount(order.ID, Convert.ToInt32(productId), 1);
                listViewOrderItems.ItemsSource = order.Items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// substracts amount of an item in an order
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event arguments</param>
        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var productId = ((Button)sender).Tag;
                order = Bl.Order.AddAmount(order.ID, Convert.ToInt32(productId), -1);
                listViewOrderItems.ItemsSource = order.Items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// deletes item from an order
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event arguments</param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var productId = ((Button)sender).Tag;
                order = Bl.Order.AddAmount(order.ID, Convert.ToInt32(productId), 0);
                listViewOrderItems.ItemsSource = order.Items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// adds product to an order
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event arguments</param>
        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductListWindow plw = new(order?.ID);
            plw.Show();
        }
    }
}
