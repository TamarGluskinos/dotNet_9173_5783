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
using PL.Order;
using BlApi;
using System.Collections.ObjectModel;

namespace PL;

/// <summary>
/// Interaction logic for OrderList.xaml
/// </summary>
public partial class OrderListWindow : Window
{
    /// <summary>
    /// BL instance
    /// </summary>
    private BlApi.IBL Bl { get; set; }

    /// <summary>
    /// observable order list
    /// </summary>
    private ObservableCollection<BO.OrderForList> orderList { get; set; }

    /// <summary>
    /// OrderListWindow constructor
    /// </summary>
    public OrderListWindow()
    {
        InitializeComponent();
        Bl = BlApi.Factory.Get();
        orderList = new ObservableCollection<BO.OrderForList>(Bl.Order.ReadOrderList());
        OrderListView.ItemsSource = orderList;
    }

    /// <summary>
    /// shows an order's details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OrderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        OrderWindow? OW = new(Bl, (OrderListView?.SelectedItem as BO.OrderForList)?.ID, true, orderList);
        OW.Show();
    }
}