using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BO;

namespace PL.PO
{
    /// <summary>
    /// Cart class as dependency object
    /// </summary>
    public class Cart : DependencyObject
    {

        /// <summary>
        /// customer's name
        /// </summary>
        public string? CustomerName
        {
            get { return (string)GetValue(CustomerNameProperty); }
            set { SetValue(CustomerNameProperty, value); }
        }

        /// <summary>
        /// customer's email
        /// </summary>
        public string? CustomerEmail
        {
            get { return (string)GetValue(CustomerEmailProperty); }
            set { SetValue(CustomerEmailProperty, value); }
        }

        /// <summary>
        /// customer's address
        /// </summary>
        public string? CustomerAddress
        {
            get { return (string)GetValue(CustomerAddressProperty); }
            set { SetValue(CustomerAddressProperty, value); }
        }

        /// <summary>
        /// list of items in cart
        /// </summary>
        public List<OrderItem?>? Items
        {
            get { return (List<OrderItem?>?)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        /// <summary>
        /// price of cart
        /// </summary>
        public double Price
        {
            get { return (double)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }

        public static readonly DependencyProperty CustomerNameProperty = DependencyProperty.Register("CustomerName", typeof(string), typeof(Cart), new UIPropertyMetadata(null));
        public static readonly DependencyProperty CustomerEmailProperty = DependencyProperty.Register("CustomerEmail", typeof(string), typeof(Cart), new UIPropertyMetadata(null));
        public static readonly DependencyProperty CustomerAddressProperty = DependencyProperty.Register("CustomerAddress", typeof(string), typeof(Cart), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(List<OrderItem?>), typeof(Cart), new UIPropertyMetadata(null));
        public static readonly DependencyProperty PriceProperty = DependencyProperty.Register("Price", typeof(double), typeof(Cart), new UIPropertyMetadata(null));
    }
}

