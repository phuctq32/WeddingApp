using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;

namespace WeddingApp.ViewModels
{
    internal class AddRoleVM : ViewModelBase
    {
        private List<FUNCTION> functions;
        private List<FUNCTION> selectedFunctions;
        public List<FUNCTION> Functions
        {
            get => functions;
            set
            {
                functions = value;
                OnPropertyChanged("Functions");
            }
        }
        public List<FUNCTION> SelectedFunctions
        {
            get => selectedFunctions;
            set { selectedFunctions = value; OnPropertyChanged("SelectedFunctions"); }
        }
        public ICommand LoadedCommand { get; set; }
        public ICommand AllCheckedCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public ICommand AddNewRoleCommand { get; set; }
        public AddRoleVM() 
        {
            AllCheckedCommand = new RelayCommand<AddRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => AllChecked(parameter));
            CheckedCommand = new RelayCommand<CheckBox>((parameter) => parameter == null ? false : true, (parameter) => Checked(parameter));
            LoadedCommand = new RelayCommand<AddRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => Load(parameter));
            AddNewRoleCommand = new RelayCommand<AddRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => AddNewRole(parameter));
        }
        private void Load(AddRoleWindow parameter)
        {
            Functions = Data.Ins.DB.FUNCTIONS.ToList();
        }
        private void AllChecked(AddRoleWindow parameter)
        {
            bool newVal = (parameter.selectAllCheckBox.IsChecked == true);
            foreach (var item in FindVisualChildren<CheckBox>(parameter.functionList))
            {
                item.IsChecked = newVal;
            }
        }
        private void Checked(CheckBox parameter)
        {
            AddRoleWindow addRoleWindow = GetAncestorOfType<AddRoleWindow>(parameter);
            bool isAllChecked = true;
            foreach (var item in FindVisualChildren<CheckBox>(addRoleWindow.functionList))
            {
                if (item.IsChecked == false)
                {
                    isAllChecked = false;
                    break;
                }
            }
            addRoleWindow.selectAllCheckBox.IsChecked = isAllChecked;
            
        }
        private void AddNewRole(AddRoleWindow parameter)
        {
            // Check if role name is empty
            if (string.IsNullOrEmpty(parameter.RoleNameTxt.Text.Trim()))
            {
                CustomMessageBox.Show("Tên chức vụ trống!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            // Check if have no function is chosen
            bool isAnyCheck = false;
            foreach (var item in FindVisualChildren<CheckBox>(parameter.functionList))
            {
                if (item.IsChecked == true)
                {
                    isAnyCheck = true;
                    break;
                }
            }
            if (!isAnyCheck)
            {
                CustomMessageBox.Show("Vui lòng chọn chức năng cho chức vụ!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (CustomMessageBox.Show("Lưu thay đổi?", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
            {
                try
                {
                    string roleName = parameter.RoleNameTxt.Text.ToString();
                    string[] strRoleName = roleName.Split(' ');
                    string roleID = "";
                    foreach (var word in strRoleName)
                    {
                        roleID += word[0].ToString().ToUpper();
                    }

                    ROLE newRole = new ROLE();
                    newRole.ROLEID = roleID;
                    newRole.ROLENAME = roleName;
                    Data.Ins.DB.ROLES.Add(newRole);

                    List<PERMISSION> currentPermissions = Data.Ins.DB.PERMISSIONs.ToList();
                    int max = -1;
                    for (int i = 0; i < currentPermissions.Count; i++)
                    {
                        if (currentPermissions[i].PERMISSIONID > max)
                        {
                            max = currentPermissions[i].PERMISSIONID;
                        }
                    }

                    List<PERMISSION> newRolePermissions = new List<PERMISSION>();
                    foreach (var lvi in FindVisualChildren<ListViewItem>(parameter.functionList))
                    {
                        FUNCTION func = lvi.DataContext as FUNCTION;
                        CheckBox checkBox = GetVisualChild<CheckBox>(lvi);
                        if (checkBox.IsChecked == true)
                        {
                            PERMISSION p = new PERMISSION();
                            p.PERMISSIONID = max++;
                            p.ROLEID = roleID;
                            p.FUNCTIONID = func.FUNCTIONID;
                            Data.Ins.DB.PERMISSIONs.Add(p);
                        }
                    }

                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Thêm thành công!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
                }
                catch
                {
                    CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", System.Windows.MessageBoxImage.Error);
                }
                
            }
        }
    }
}
