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
    internal class ChangeEmployeeVM : ViewModelBase
    {
        
        public ICommand ChangeEmployeeCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RePasswordChangedCommand { get; set; }

        private string employeeName;

        public string EmployeeName
        { get => employeeName; set { employeeName = value; OnPropertyChanged(); } }

        private string roleid;

        public string Roleid { get => roleid; set { roleid = value; OnPropertyChanged(); } }

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

        public EMPLOYEE current_Employee;

        public DateTime Date
        {
            get => date; set { date = value; OnPropertyChanged(); }
        }

        public ChangeEmployeeVM()
        {
            
            LoadedCommand = new RelayCommand<ChangeEmployeeInformationWindow>(parameter => true, parameter => Loaded(parameter));
            ChangeEmployeeCommand = new RelayCommand<ChangeEmployeeInformationWindow>((parameter) => { return true; }, (parameter) => SaveChangesEmployee(parameter));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { Password = parameter.Password; });
            RePasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { RePassword = parameter.Password; });

        }
        
        public void SaveChangesEmployee(ChangeEmployeeInformationWindow parameter)
        {

             EMPLOYEE employee = Data.Ins.DB.EMPLOYEES.Where(x => x.USERNAME == parameter.txtUsername.Text).SingleOrDefault();

            //CustomMessageBox.Show(employee.EMPLOYEENAME, MessageBoxButton.OK, MessageBoxImage.Warning);
            // Check NAME
            if (string.IsNullOrEmpty(parameter.txtEmployeeName.Text))
            {
                parameter.txtEmployeeName.Focus();
                CustomMessageBox.Show("Họ và tên đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            //Check Password
            if (!string.IsNullOrEmpty(parameter.PasswordBox.Password))
            {
                parameter.PasswordBox.Focus();
                parameter.PasswordBox.Password = "";
                //CustomMessageBox.Show("Mật khẩu trống", MessageBoxButton.OK, MessageBoxImage.Warning);
                employee.PASSWORD = Password;
                return;
            }
            if (parameter.PasswordBox.Password.Contains(" "))
            {
                parameter.PasswordBox.Focus();
                CustomMessageBox.Show("Mật khẩu không được chứa hoảng trắng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //check repassword
            
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

    
            try
            {
                //try to update database
                employee.EMPLOYEENAME = parameter.txtEmployeeName.Text;
                employee.USERNAME = parameter.txtUsername.Text;
                employee.SALARY = Convert.ToInt32(parameter.txtSalary.Text);
                Data.Ins.DB.SaveChanges();
                CustomMessageBox.Show("Thay đổi thành công!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                CustomMessageBox.Show("Thay đổi không thành công!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            parameter.Close();
        }
    }
    
}
