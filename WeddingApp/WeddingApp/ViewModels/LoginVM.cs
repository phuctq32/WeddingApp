using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;
using WeddingApp.Views.UserControls;

namespace WeddingApp.ViewModels
{
    internal class LoginVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        public ICommand OpenLogInWDCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        private string password;

        public string Password
        { get => password; set { password = value; OnPropertyChanged(); } }

        private string userName;

        public string UserName
        { get => userName; set { userName = value; OnPropertyChanged(); } }

        private bool isLogin;
        public bool IsLogin { get => isLogin; set => isLogin = value; }

        public LoginVM()
        {
            Password = "";
            OpenLogInWDCommand = new RelayCommand<LoginWindow>((parameter) => { return true; }, (parameter) => Login(parameter));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((parameter) => { return true; }, (parameter) => { Password = parameter.Password; });
            LoadedCommand = new RelayCommand<ControlBarUC>(p => true, (p) => Loaded(p));
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
            // set default date picker
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
        }

        public void Login(LoginWindow parameter)
        {
            try
            {
                isLogin = false;
                if (parameter == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(parameter.txtUsername.Text))
                {
                    CustomMessageBox.Show("Vui lòng nhập tên đăng nhập!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.txtUsername.Focus();
                    return;
                }
                if (parameter.txtUsername.Text.Contains(" "))
                {
                    CustomMessageBox.Show("Tên đăng nhập không được chứa khoảng trắng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.txtUsername.Focus();
                    return;
                }
                //check password
                if (string.IsNullOrEmpty(parameter.txtPassword.Password))
                {
                    CustomMessageBox.Show("Vui lòng nhập mật khẩu!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.txtPassword.Focus();
                    return;
                }
                if (parameter.txtPassword.Password.Contains(" "))
                {
                    CustomMessageBox.Show("Mật khẩu không được chứa khoảng trắng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    parameter.txtPassword.Focus();
                    return;
                }

                string passEncode = MD5Hash(Base64Encode(Password));
                int accCount = Data.Ins.DB.EMPLOYEES.Where(x => x.USERNAME == UserName && x.PASSWORD == passEncode).Count();

                if (accCount > 0)
                {
                    // check username
                    string tempUsername = Data.Ins.DB.EMPLOYEES.Where(x => x.USERNAME == UserName && x.PASSWORD == passEncode).SingleOrDefault().USERNAME;
                    if (tempUsername != UserName)
                    {
                        isLogin = false;
                        CustomMessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", MessageBoxButton.OK, MessageBoxImage.Error);
                        parameter.txtPassword.Focus();
                    }
                    else
                    {
                        isLogin = true;
                        CurrentAccount.Employee = Data.Ins.DB.EMPLOYEES.Where(x => x.USERNAME == UserName && x.PASSWORD == passEncode).SingleOrDefault();
                        //if (CurrentAccount.User.TYPE == "admin")
                        //{
                        //    CurrentAccount.IsAdmin = true;
                        //    CurrentAccount.IsUser = false;
                        //}
                        //else
                        //{
                        //    CurrentAccount.IsAdmin = false;
                        //    CurrentAccount.IsUser = true;
                        //}
                        CurrentAccount.Username = CurrentAccount.Employee.USERNAME;
                        CurrentAccount.Role = CurrentAccount.Employee.ROLEID;
                        MainWindow app = new MainWindow();
                        parameter.Close();
                        app.ShowDialog();
                    }
                }
                else
                {
                    isLogin = false;
                    CustomMessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", MessageBoxButton.OK, MessageBoxImage.Error);
                    parameter.txtPassword.Focus();
                }
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu");
            }
        }

        public void Loaded(ControlBarUC cb)
        {
            UserName = "";
            cb.closeBtn.Command = CloseWindowCommand;
            cb.closeBtn.CommandParameter = cb;
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