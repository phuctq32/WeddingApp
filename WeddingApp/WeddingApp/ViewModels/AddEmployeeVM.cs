using System;
using System.Collections.Generic;
using System.Globalization;
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
    class AddEmployeeVM : ViewModelBase
    {
        public ICommand AddEmployeeCommand { get; set; }


        public ICommand selectCommand { get; set; }

        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RePasswordChangedCommand { get; set; }

        private string employeeName;

        public string EmployeeName
        { get => employeeName; set { employeeName = value; OnPropertyChanged(); } }

        private string userName;

        public string UserName
        { get => userName; set { userName = value; OnPropertyChanged(); } }
        
        private string roleid;

        public string Roleid
        { get => roleid; set { roleid = value; OnPropertyChanged(); } }

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
        private string strdate;
        public string strDate
        { get => strdate; set { strdate = value; OnPropertyChanged(); } }

        IFormatProvider culture = new CultureInfo("vi-VN", true);
        public AddEmployeeVM()
        {
            selectCommand = new RelayCommand<ComboBox>((parameter) => true, (parameter) => Select(parameter));
            AddEmployeeCommand = new RelayCommand<AddEmployeeWindow>((parameter) => { return true; }, (parameter) => AddEmployee(parameter));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { Password = parameter.Password; });
            RePasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { RePassword = parameter.Password; });

        }
        private void Select(ComboBox item)
        {

            if (item.SelectedIndex == 0)
                Roleid = "NV";
            else
                if (item.SelectedIndex == 1)
                Roleid = "GD";

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
            // Check 
            /*if(parameter.comboBoxRoleList.SelectedIndex == 0)
            {

            }    
            if (string.IsNullOrEmpty(Roleid))
            {
                parameter.comboBoxRoleList.Focus();
                CustomMessageBox.Show("Chưa chọn chức vụ!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }*/

            string passEncode = MD5Hash(Base64Encode(Password));
            Data.Ins.DB.EMPLOYEES.Add(new EMPLOYEE() { EMPLOYEENAME = EmployeeName, USERNAME = UserName, PASSWORD = passEncode, SALARY = Salary, STARTWORKING = Date, ROLEID = "NV" });
            Data.Ins.DB.SaveChanges();
            CustomMessageBox.Show("Thêm nhân viên thành công", MessageBoxButton.OK);
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
