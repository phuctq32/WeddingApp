using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    class EmployeeListVM : ViewModelBase
    {
        public ICommand OpenAddEmployeeWindowCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }

        private string employeeName;

        public string EmployeeName
        { get => employeeName; set { employeeName = value; OnPropertyChanged(); } }

        private string userName;

        public string UserName
        { get => userName; set { userName = value; OnPropertyChanged(); } }

        private string password;

        public string Password
        { get => password; set { password = value; OnPropertyChanged(); } }

        private string salary;

        public string Salary
        { get => salary; set { salary = value; OnPropertyChanged(); } }

        private DateTime date = DateTime.Now;

        public DateTime Date
        { get => date; set { date = value; OnPropertyChanged(); } }

        public EmployeeListVM()
        {
            OpenAddEmployeeWindowCommand = new RelayCommand<EmployeeListUC>((parameter) => { return true; }, (parameter) => OpenAddEmployee(parameter));
            AddEmployeeCommand = new RelayCommand<AddEmployeeWindow>((parameter) => { return true; }, (parameter) => AddEmployee(parameter));
        }

        public void OpenAddEmployee(EmployeeListUC parameter)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.ShowDialog();
        } 
        public void AddEmployee(AddEmployeeWindow parameter)
        {
            // Check username
            if (string.IsNullOrEmpty(parameter.txtUsername.Text))
            {
                parameter.txtUsername.Focus();
                CustomMessageBox.Show("Tài khoản đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (parameter.txtUsername.Text.Contains(" "))
            {
                parameter.txtUsername.Focus();
                CustomMessageBox.Show("Tài khoản không được chứa khoảng trắng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(parameter.txtUsername.Text, @"^[a-zA-Z0-9_]+$"))
            {
                parameter.txtUsername.Focus();
                return;
            }
           /* // Check User exist
            int accCount = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == UserName.Trim()).Count();
            if (accCount > 0)
            {
                parameter.txtUsername.Focus();
                CustomMessageBox.Show("Tài khoản đã tồn tại!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }*/

            //Check Password
            if (string.IsNullOrEmpty(parameter.PasswordBox.Password))
            {
                parameter.PasswordBox.Focus();
                parameter.PasswordBox.Password = "";
                CustomMessageBox.Show("Mật khẩu trống", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (parameter.PasswordBox.Password.Contains(" "))
            {
                parameter.PasswordBox.Focus();
                CustomMessageBox.Show("Mật khẩu không được chứak hoảng trắng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CustomMessageBox.Show("Thành công" + parameter.txtDate.Text.ToString());
        }
    }
}
