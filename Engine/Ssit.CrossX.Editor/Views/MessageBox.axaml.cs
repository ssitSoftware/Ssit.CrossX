using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Ssit.CrtossX.Editor.Helpers;

namespace Ssit.CrtossX.Editor.Views
{
    public partial class MessageBox : Window
    {

        public MessageBox()
        {
            InitializeComponent();
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close(false);
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close(null);
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            WindowHelpers.FitDialog(this, Panel);
        }
    }
}