using System.Windows;
using WeddingApp.Views;

namespace WeddingApp
{
    internal class CustomMessageBox
    {
        public static MessageBoxResult Show(string message)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(message);
            msg.ShowDialog();

            return msg.Result;
        }

        public static MessageBoxResult Show(string message, MessageBoxButton button)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(message, button);
            msg.ShowDialog();

            return msg.Result;
        }

        public static MessageBoxResult Show(string message, MessageBoxImage icon)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(message, icon);
            msg.ShowDialog();

            return msg.Result;
        }

        public static MessageBoxResult Show(string message, MessageBoxButton button, MessageBoxImage icon)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(message, button, icon);
            msg.ShowDialog();

            return msg.Result;
        }
    }
}