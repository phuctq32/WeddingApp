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
    internal class EditRoleVM : ViewModelBase
    {
        private ROLE currentRole;
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
        public ICommand EditRoleCommand { get; set; }
        public EditRoleVM()
        {
            AllCheckedCommand = new RelayCommand<EditRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => AllChecked(parameter));
            CheckedCommand = new RelayCommand<CheckBox>((parameter) => parameter == null ? false : true, (parameter) => Checked(parameter));
            LoadedCommand = new RelayCommand<EditRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => Load(parameter));
            EditRoleCommand = new RelayCommand<EditRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => EditRole(parameter));
        }
        private void Load(EditRoleWindow parameter)
        {
            Functions = Data.Ins.DB.FUNCTIONS.ToList();
            currentRole = Data.Ins.DB.ROLES.Where(role => role.ROLENAME == parameter.RoleNameTxt.Text.ToString()).SingleOrDefault();
        }
        private void AllChecked(EditRoleWindow parameter)
        {
            bool newVal = (parameter.selectAllCheckBox.IsChecked == true);
            foreach (var item in FindVisualChildren<CheckBox>(parameter.functionList))
            {
                item.IsChecked = newVal;
            }
        }
        private void Checked(CheckBox parameter)
        {
            EditRoleWindow editRoleWindow = GetAncestorOfType<EditRoleWindow>(parameter);
            bool isAllChecked = true;
            foreach (var item in FindVisualChildren<CheckBox>(editRoleWindow.functionList))
            {
                if (item.IsChecked == false)
                {
                    isAllChecked = false;
                    break;
                }
            }
            editRoleWindow.selectAllCheckBox.IsChecked = isAllChecked;

        }
        private void EditRole(EditRoleWindow parameter)
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
                    currentRole.ROLENAME = parameter.RoleNameTxt.Text.Trim();
                    List<PERMISSION> currentPermissions = Data.Ins.DB.PERMISSIONs.ToList();
                    int max = -1;
                    for (int i = 0; i < currentPermissions.Count; i++)
                    {
                        if (currentPermissions[i].PERMISSIONID > max)
                        {
                            max = currentPermissions[i].PERMISSIONID;
                        }
                    }

                    List<PERMISSION> editedRolePermissions = new List<PERMISSION>();
                    foreach (var lvi in FindVisualChildren<ListViewItem>(parameter.functionList))
                    {
                        FUNCTION func = lvi.DataContext as FUNCTION;
                        CheckBox checkBox = GetVisualChild<CheckBox>(lvi);
                        if (checkBox.IsChecked == true)
                        {
                            PERMISSION p = new PERMISSION();
                            p.PERMISSIONID = max++;
                            p.FUNCTIONID = func.FUNCTIONID;
                            if (!Data.Ins.DB.PERMISSIONs.Contains(p))
                            {
                                Data.Ins.DB.PERMISSIONs.Add(p);
                            }
                            else
                            {
                                max--;
                            }
                        }
                    }
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Lưu thành công!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
                }
                catch
                {
                    CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", System.Windows.MessageBoxImage.Error);
                }
                
            }
        }
    }
}
