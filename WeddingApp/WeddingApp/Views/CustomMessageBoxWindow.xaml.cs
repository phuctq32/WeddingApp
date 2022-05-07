using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WeddingApp.Views
{
    /// <summary>
    /// Interaction logic for CustomMessageBoxWindow.xaml
    /// </summary>
    public partial class CustomMessageBoxWindow : Window
    {
        internal CustomMessageBoxWindow(string message)
        {
            InitializeComponent();
            this.controlBar.centerBar.Visibility = Visibility.Hidden;
            this.controlBar.minimizeBtn.Visibility = Visibility.Hidden;

            Message = message;
        }

        internal CustomMessageBoxWindow(string message, MessageBoxImage image)
        {
            InitializeComponent();
            this.controlBar.centerBar.Visibility = Visibility.Hidden;
            this.controlBar.minimizeBtn.Visibility = Visibility.Hidden;

            Message = message;
            DisplayImageIcon(image);
        }

        internal CustomMessageBoxWindow(string message, MessageBoxButton button)
        {
            InitializeComponent();
            this.controlBar.centerBar.Visibility = Visibility.Hidden;
            this.controlBar.minimizeBtn.Visibility = Visibility.Hidden;

            Message = message;
            DisplayButton(button);
        }

        internal CustomMessageBoxWindow(string message, MessageBoxButton button, MessageBoxImage image)
        {
            InitializeComponent();
            this.controlBar.centerBar.Visibility = Visibility.Hidden;
            this.controlBar.minimizeBtn.Visibility = Visibility.Hidden;

            Message = message;
            DisplayButton(button);
            DisplayImageIcon(image);
        }

        internal string Message { get => textblock.Text; set => textblock.Text = value; }

        public MessageBoxResult Result { get; set; }

        private void DisplayButton(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    this.btn_OK.Visibility = Visibility.Visible;
                    this.btn_Cancel.Visibility = Visibility.Visible;
                    this.btn_OK.Focus();
                    break;

                case MessageBoxButton.OK:
                    this.btn_OK.Visibility = Visibility.Visible;
                    this.btn_OK.Focus();
                    break;

                default:
                    break;
            }
        }

        private void DisplayImageIcon(MessageBoxImage image)
        {
            BitmapImage bitmapImage = new BitmapImage();

            switch (image)
            {
                case MessageBoxImage.Warning:
                    bitmapImage = new BitmapImage(new Uri("pack://application:,,,/WeddingApp;component/Resources/Images/warning_icon.png"));
                    break;

                case MessageBoxImage.Question:
                    bitmapImage = new BitmapImage(new Uri("pack://application:,,,/WeddingApp;component/Resources/Images/question_icon.png"));
                    break;

                case MessageBoxImage.Error:
                    bitmapImage = new BitmapImage(new Uri("pack://application:,,,/WeddingApp;component/Resources/Images/error_icon.png"));
                    break;

                case MessageBoxImage.Asterisk:
                    bitmapImage = new BitmapImage(new Uri("pack://application:,,,/WeddingApp;component/Resources/Images/success_icon.png"));
                    break;

                case MessageBoxImage.None:
                    bitmapImage = new BitmapImage(new Uri("pack://application:,,,/WeddingApp;component/Resources/Images/information_icon.png"));
                    break;

                default:
                    break;
            }
            this.image_icon.Source = bitmapImage;
            this.image_icon.Visibility = Visibility.Visible;
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            this.Close();
        }
    }
}