using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    class EmployeeListVM : ViewModelBase
    {
        public ICommand OpenAddEmployeeWindowCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }

        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RePasswordChangedCommand { get; set; }

        private string employeeName;

        public string EmployeeName
        { get => employeeName; set { employeeName = value; OnPropertyChanged(); } }

        private string userName;

        public string UserName
        { get => userName; set { userName = value; OnPropertyChanged(); } }

        private string password;

        public string Password
        { get => password; set { password = value; OnPropertyChanged(); } }


        private string rePassword;

        public string RePassword
        { get => rePassword; set { rePassword = value; OnPropertyChanged(); } }

        private int salary;

        public int Salary
        { get => salary; set { salary = value; OnPropertyChanged(); } }

        private DateTime date = DateTime.Now;

        public DateTime Date
        { get => date; set { date = value; OnPropertyChanged(); } }

        public EmployeeListVM()
        {
            OpenAddEmployeeWindowCommand = new RelayCommand<EmployeeListUC>((parameter) => { return true; }, (parameter) => OpenAddEmployee(parameter));
            AddEmployeeCommand = new RelayCommand<AddEmployeeWindow>((parameter) => { return true; }, (parameter) => AddEmployee(parameter));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { Password = parameter.Password; });
            RePasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { RePassword = parameter.Password; });

        }

        public void OpenAddEmployee(EmployeeListUC parameter)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.ShowDialog();
        } 
        public void AddEmployee(AddEmployeeWindow parameter)
        {
            // Check NAME
            if (string.IsNullOrEmpty(parameter.txtEmployeeName.Text))
            {
                parameter.txtEmployeeName.Focus();
                CustomMessageBox.Show("Họ và tên đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
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
            // Check User exist
            int accCount = Data.Ins.DB.EMPLOYEES.Where(x => x.USERNAME == UserName.Trim()).Count();
            if (accCount > 0)
            {
                parameter.txtUsername.Focus();
                CustomMessageBox.Show("Tài khoản đã tồn tại!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
                CustomMessageBox.Show("Mật khẩu không được chứa hoảng trắng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //check repassword
            if (string.IsNullOrEmpty(parameter.RePasswordBox.Password))
            {
                parameter.RePasswordBox.Focus();
                parameter.RePasswordBox.Password = "";
                CustomMessageBox.Show("Chưa xác nhận mật khẩu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (parameter.RePasswordBox.Password.Contains(" "))
            {
                parameter.RePasswordBox.Focus();
                CustomMessageBox.Show("Nhập lại mật khẩu không đúng", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Password != RePassword)
            {
                parameter.RePasswordBox.Focus();
                CustomMessageBox.Show("Nhập lại mật khẩu không đúng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check SALARY
            if (string.IsNullOrEmpty(parameter.txtSalary.Text))
            {
                parameter.txtSalary.Focus();
                CustomMessageBox.Show("Lương đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!Regex.IsMatch(parameter.txtSalary.Text, @"^[0-9_]+$"))
            {
                parameter.txtSalary.Focus();
                CustomMessageBox.Show("Lương chỉ được nhập số!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Data.Ins.DB.EMPLOYEES.Add(new EMPLOYEE() { EMPLOYEENAME = EmployeeName, USERNAME = UserName, PASSWORD = Password, SALARY = Salary ,STARTWORKING = Date, ROLEID = "NV"});
            Data.Ins.DB.SaveChanges();
            CustomMessageBox.Show("Thêm nhân viên thành công", MessageBoxButton.OK);
        }
    }
}
