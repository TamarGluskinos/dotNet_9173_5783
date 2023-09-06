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

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }
      
        /// <summary>
        /// move from MainWindow to ProductListWindow
        /// </summary>
        /// <param name="sender">btnShowViewList</param>
        /// <param name="e">button clicked</param>
        private void ShowProductList_Click(object sender, RoutedEventArgs e)
        {
            ProductListWindow PLW = new();
            PLW.Show();
        }

        private void ShowOrderList_Click(object sender, RoutedEventArgs e)
        {
            OrderListWindow OLW = new();
            OLW.Show();
        }
    }
}
