using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    internal class ServiceListVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand OpenAddServiceListWindowCommand { get; set; }
        public ICommand OpenEditServiceListWindowCommand { get; set; }
        public ICommand DeleteServiceCommand { get; set; }

        private List<SERVICE> listService;

        public List<SERVICE> ListService
        {
            get => listService;
            set
            {
                listService = value;
                OnPropertyChanged("ListService");
            }
        }

        public ServiceListUC serviceListUC;

        public ServiceListVM()
        {
            LoadedCommand = new RelayCommand<ServiceListUC>((parameter) => { return true; }, (parameter) => load(parameter));
            OpenAddServiceListWindowCommand = new RelayCommand<ServiceListUC>((parameter) => { return true; }, (parameter) => OpenAddServiceListWindow(parameter));
            OpenEditServiceListWindowCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => OpenEditServiceListWindow(parameter));
        }

        public void load(ServiceListUC parameter)
        {
            ListService = Data.Ins.DB.SERVICEs.ToList();
            serviceListUC = parameter;
        }

        public void OpenAddServiceListWindow(ServiceListUC parameter)
        {
            AddServiceWindow addServiceWindow = new AddServiceWindow();
            addServiceWindow.ShowDialog();
            load(serviceListUC);
        }

        public void OpenEditServiceListWindow(ListViewItem listViewItem)
        {
            SERVICE service = listViewItem.DataContext as SERVICE;
            EditServiceWindow editServiceWindow = new EditServiceWindow();
            editServiceWindow.txtName.Text = service.SERVICENAME;
            editServiceWindow.txtCost.Text = Convert.ToInt32(service.COST).ToString();
            editServiceWindow.txtDescription.Text = service.SERVICEDESCRIPTION;
            editServiceWindow.ShowDialog();
            load(serviceListUC);
        }

        protected void Delete(ListViewItem listViewItem)
        {
            SERVICE service = listViewItem.DataContext as SERVICE;
            if (CustomMessageBox.Show("Xóa dịch vụ ?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Data.Ins.DB.SERVICEs.Remove(service);
                Data.Ins.DB.SaveChanges();
                ListService = Data.Ins.DB.SERVICEs.ToList();
                CustomMessageBox.Show("Xóa thành công", MessageBoxButton.OK);
            }
        }
    }
}