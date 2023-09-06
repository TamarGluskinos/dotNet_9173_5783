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
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// BL instance
    /// </summary>
    private BlApi.IBL Bl { get; set; }

    /// <summary>
    /// MainWindow constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        Bl = BlApi.Factory.Get();
    }

    /// <summary>
    /// moves administrator to his window
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void Admin_Click(object sender, RoutedEventArgs e)
    {
        AdminWindow admin= new();
        admin.Show();
    }

    /// <summary>
    /// moves to NewOrderWindow
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void ButtonNewOrder_Click(object sender, RoutedEventArgs e)
    {
        NewOrderWindow now = new();
        now.Show();
    }

    /// <summary>
    /// moves to OrderTrackingWindow to track a window
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event arguments</param>
    private void ButtonOrderTrack_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int orderId = Convert.ToInt32(txtOrderId.Text);
            OrderTrackingWindow otw = new(orderId, this);
            otw.Show();
            this.Hide();
        }
        catch (BlApi.BlEntityNotFoundException ex)
        {
            MessageBox.Show("no such order");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// moves to SimulatorWindow to start a simulation
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">event argument</param>
    private void btnStartSimulator_Click(object sender, RoutedEventArgs e)
    {
        SimulatorWindow sw = new();
        sw.Show();
    }
}

