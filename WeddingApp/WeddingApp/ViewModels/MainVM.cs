using WeddingApp.Models;
using WeddingApp.Views;
using WeddingApp.Views.UserControls;
using WeddingApp.Views.UserControls.Admin;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace WeddingApp.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        public ICommand SwitchTabCommand { get; set; }
        public ICommand LogOutCommand { get; set; }

        //public string Fullname
        //{ get => CurrentAccount.User.FULLNAME_; set { CurrentAccount.User.FULLNAME_ = value; OnPropertyChanged("Fullname"); } }
        //public string Avatar
        //{ get => CurrentAccount.User.AVATAR_; set { CurrentAccount.User.AVATAR_ = value; OnPropertyChanged("Avatar"); } }

        private string mail;

        public string Mail
        { get => mail; set { mail = value; OnPropertyChanged("Mail"); } }

        private string phone;

        public string Phone
        { get => phone; set { phone = value; OnPropertyChanged("Phone"); } }

        private string userName;

        public string UserName
        { get => userName; set { userName = value; OnPropertyChanged("UserName"); } }

        private string address;

        public string Address
        { get => address; set { address = value; OnPropertyChanged("Address"); } }

        public MainViewModel()
        {
            LoadedCommand = new RelayCommand<MainWindow>(parameter => true, parameter => Loaded(parameter));
            CloseWindowCommand = CloseWindowCommand = new RelayCommand<UserControl>((p) => p == null ? false : true, p =>
            {
                if (CustomMessageBox.Show("Thoát ứng dụng?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    FrameworkElement window = ControlBarVM.GetParentWindow(p);
                    var w = window as Window;
                    if (w != null)
                    {
                        w.Close();
                    }
                }
            });
            SwitchTabCommand = new RelayCommand<MainWindow>(p => true, (p) => SwitchTab(p));
            LogOutCommand = new RelayCommand<MainWindow>(p => true, (p) => LogOut(p));
        }

        private void Loaded(MainWindow mainWindow)
        {

            List<PERMISSION> list = Data.Ins.DB.PERMISSIONs.Where(x => x.ROLEID == CurrentAccount.Role).ToList();
            List<string> FunctionName = new List<string>();
            foreach (var item in list)
            {
                FunctionName.Add(item.FUNCTIONID.ToString());
            }
            mainWindow.listViewMenu.Items.Cast<ListViewItem>().ToList().ForEach(item =>
            {
                if (FunctionName.Contains(item.Name))
                    item.Visibility = Visibility.Visible;
            });
            if (CurrentAccount.Role == "NV")
            {
                mainWindow.ucWindow.Children.Add(new MenuUC());
            }
            else
            {
                mainWindow.ucWindow.Children.Add(new DashBoardUC());
            }
            mainWindow.controlBar.closeBtn.Command = CloseWindowCommand;
            mainWindow.controlBar.closeBtn.CommandParameter = mainWindow.controlBar;
        }

        private void SwitchTab(MainWindow mainWindow)
        {
            int index = mainWindow.listViewMenu.SelectedIndex;
            List<ListViewItem> listViewItems  = mainWindow.listViewMenu.Items.Cast<ListViewItem>().ToList();
            ListViewItem listViewItem = listViewItems[index];
            switch (listViewItem.Name)
            {
                case "WeddingHallUC":
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new WeddingHallUC());
                    break;

                case "WeddingListUC":
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new WeddingListUC());
                    break;

                case "InvoiceListUC":
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new InvoiceListUC());
                    break;

                case 3:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new AccountUC());
                    break;

                case 4:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new ContactUC());
                    break;

                case 5:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new DashBoardUC());
                    break;

                case 6:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new EditProductUC());
                    break;

                case 7:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new OrderManagementUC());
                    break;

                case 8:
                    mainWindow.ucWindow.Children.Clear();
                    mainWindow.ucWindow.Children.Add(new AccountUC());
                    break;
            }
        }
        private void LogOut(MainWindow mainWindow)
        {
            if (CustomMessageBox.Show("Bạn có muốn đăng xuất?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                mainWindow.Close();
            }
        }
    }
}