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
    internal class EditServiceVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand EditServiceCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }

        public EditServiceWindow editServiceWD;

        private SERVICE service;

        public EditServiceVM()
        {
            LoadedCommand = new RelayCommand<EditServiceWindow>((parameter) => { return true; }, (parameter) => Load(parameter));
            EditServiceCommand = new RelayCommand<EditServiceWindow>((parameter) => { return true; }, (parameter) => editService(parameter));
            CloseButtonCommand = new RelayCommand<EditServiceWindow>((parameter) => { return true; }, (parameter) => CloseButton(parameter));
        }

        public void Load(EditServiceWindow parameter)
        {
            editServiceWD = parameter;
            service = Data.Ins.DB.SERVICEs.Where(x => x.SERVICENAME == parameter.txtName.Text).SingleOrDefault();
            editServiceWD.txtCost.Text = String.Format("{0:N0}", editServiceWD.txtCost.Text);
        }

        public void editService(EditServiceWindow parameter)
        {
            // Check NAME
            if (string.IsNullOrEmpty(parameter.txtName.Text))
            {
                parameter.txtName.Focus();
                CustomMessageBox.Show("Tên dịch vụ đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!Regex.IsMatch(parameter.txtCost.Text, @"^[0-9_]+$"))
            {
                parameter.txtCost.Focus();
                CustomMessageBox.Show("Giá không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtCost.Text = "";
                return;
            }
            try
            {
                //try to update database
                service.SERVICENAME = parameter.txtName.Text;
                service.COST = Convert.ToInt32(parameter.txtCost.Text);
                service.SERVICEDESCRIPTION = parameter.txtDescription.Text;
                Data.Ins.DB.SaveChanges();
                CustomMessageBox.Show("Thay đổi thành công!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                CustomMessageBox.Show("Thay đổi không thành công!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            parameter.Close();
        }

        public void CloseButton(EditServiceWindow parameter)
        {
            parameter.Close();
        }
    }
}