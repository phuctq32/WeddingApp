using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    internal class ServiceListVM : ViewModelBase
    {
        public ICommand OpenAddServiceListWindowCommand { get; set; }
        public ServiceListVM()
        {
            OpenAddServiceListWindowCommand = new RelayCommand<ServiceListUC>((parameter) => { return true; }, (parameter) => OpenAddServiceListWindow(parameter));
        }

        public void OpenAddServiceListWindow(ServiceListUC parameter)
        {
            AddServiceWindow addServiceWindow = new AddServiceWindow();
            addServiceWindow.ShowDialog();
        }
    }
}
