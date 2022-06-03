using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    internal class RoleListVM : ViewModelBase
    {
        private List<ROLE> roles;
        public List<ROLE> Roles
        {
            get => roles;
            set
            {
                roles = value;
                OnPropertyChanged("Roles");
            }
        }
        public ICommand LoadedCommand { get; set; }
        public ICommand AddRoleCommand { get; set; }
        public ICommand EditRoleCommand { get; set; }
        public ICommand DeleteRoleCommand { get; set; }

        public RoleListVM()
        {
            LoadedCommand = new RelayCommand<RoleListUC>((parameter) => parameter == null ? false : true, (parameter) => Load(parameter));
            AddRoleCommand = new RelayCommand<RoleListUC>((parameter) => parameter == null ? false : true, (parameter) => AddRole(parameter));
            EditRoleCommand = new RelayCommand<ListViewItem>((parameter) => parameter == null ? false : true, (parameter) => EditRole(parameter));
        }
        private void Load(RoleListUC parameter)
        {
            Roles = Data.Ins.DB.ROLES.ToList();
        }
        private void AddRole(RoleListUC parameter)
        {
            AddRoleWindow addRoleWindow = new AddRoleWindow();
            addRoleWindow.ShowDialog();
        }
        private void EditRole(ListViewItem parameter)
        {
            ROLE role = parameter.DataContext as ROLE;
            List<PERMISSION> permissions = Data.Ins.DB.PERMISSIONs.Where(x => x.ROLEID == role.ROLEID).ToList();


            EditRoleWindow editRoleWindow = new EditRoleWindow();

            //editRoleWindow.functionList.ItemsSource = Data.Ins.DB.FUNCTIONS.ToList();
            editRoleWindow.RoleNameTxt.Text = role.ROLENAME.ToString();
            editRoleWindow.Show();
            foreach (var p in permissions)
            {
                foreach (var lvi in FindVisualChildren<ListViewItem>(editRoleWindow.functionList))
                {
                    FUNCTION func = lvi.DataContext as FUNCTION;
                    if (p.FUNCTIONID == func.FUNCTIONID)
                    {
                        CheckBox checkBox = GetVisualChild<CheckBox>(lvi);
                        checkBox.IsChecked = true;
                    }
                }
            }
        }
        private void DeleteRole(ListViewItem parameter)
        {
            if (CustomMessageBox.Show("Xóa chức vụ?", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
            {
                try
                {
                    ROLE roleToRemove = parameter.DataContext as ROLE;
                    Data.Ins.DB.ROLES.Remove(roleToRemove);
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Xóa thành công!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
                }
                catch
                {
                    CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", System.Windows.MessageBoxImage.Error);
                }
            }
        }
    }
}
