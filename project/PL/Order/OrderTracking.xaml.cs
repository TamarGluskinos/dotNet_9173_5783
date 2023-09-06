using BlApi;
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

namespace PL.Order;

/// <summary>
/// Interaction logic for OrderTracking.xaml
/// </summary>
public partial class OrderTrackingWindow : Window
{
    /// <summary>
    /// BL instance
    /// </summary>
    private BlApi.IBL Bl { get; set; }

    /// <summary>
    /// OrderTracking instance
    /// </summary>
    private BO.OrderTracking orderTracking { get; set; }

    /// <summary>
    /// the main window instance
    /// </summary>
    private MainWindow mainWindow { get; set; }


    /// <summary>
    /// OrderTrackingWindow constructor
    /// </summary>
    /// <param name="orderId">order ID</param>
    public OrderTrackingWindow(int orderId, MainWindow mWindow)
    {
        InitializeComponent();
        Bl = BlApi.Factory.Get();
        orderTracking = Bl.Order.TrackOrder(orderId);
        DataContext = orderTracking;
        TrackListView.ItemsSource = orderTracking.TrackList;
        mainWindow = mWindow;
    }

    /// <summary>
    /// tracks a specific order
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void btnOrder_Click(object sender, RoutedEventArgs e)
    {
        OrderWindow ow = new(Bl, orderTracking.ID, false);
        ow.Show();
    }

    private void btnBack_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
        mainWindow.txtOrderId.Text = null;
        mainWindow.Show();
    }
}

