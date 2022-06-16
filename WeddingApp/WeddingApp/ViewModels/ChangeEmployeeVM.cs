using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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


        public ICommand selectCommand { get; set; }
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

            //LoadedCommand = new RelayCommand<ChangeEmployeeInformationWindow>(parameter => true, parameter => Loaded(parameter));
            //selectCommand = new RelayCommand<ComboBox>((parameter) => true, (parameter) => Select(parameter));
            ChangeEmployeeCommand = new RelayCommand<ChangeEmployeeInformationWindow>((parameter) => { return true; }, (parameter) => SaveChangesEmployee(parameter));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { Password = parameter.Password; });
            RePasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { RePassword = parameter.Password; });
            
        }

        
        //    private void Select(ComboBox item)
        //{

        //    if (item.SelectedIndex == 0)
        //        Roleid = "NV";
        //    else
        //        if (item.SelectedIndex == 1)
        //        Roleid = "GD";

        //}
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
            if (string.IsNullOrEmpty(parameter.PasswordBox.Password))
            {
                parameter.PasswordBox.Focus();
                parameter.PasswordBox.Password = "";
                CustomMessageBox.Show("Mật khẩu trống", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if(!string.IsNullOrEmpty(parameter.comboBoxRoleList.Text))
            {
                CustomMessageBox.Show("Chức vụ đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            string passEncode = MD5Hash(Base64Encode(Password));

            try
            {
               

                //try to update database
                employee.EMPLOYEENAME = parameter.txtEmployeeName.Text;
                employee.USERNAME = parameter.txtUsername.Text;
                employee.ROLEID = Data.Ins.DB.ROLES.Where(x => x.ROLENAME == parameter.comboBoxRoleList.Text).SingleOrDefault().ROLEID;
                employee.PASSWORD = passEncode;
                employee.STARTWORKINGDAY = Date;
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

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
    
}
