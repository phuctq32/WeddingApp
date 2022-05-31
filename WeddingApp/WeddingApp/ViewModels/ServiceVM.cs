using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views.UserControls;

namespace WeddingApp.ViewModels
{
    internal class ServiceVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand PreviousUCCommand { get; set; }

        public ServiceVM()
        {
            LoadedCommand = new RelayCommand<ServiceSelectionUC>(p => true, p => Loaded(p));
            PreviousUCCommand = new RelayCommand<ServiceSelectionUC>(p => true, p => PreviousUC());
        }
        public void Loaded(ServiceSelectionUC serviceSelectionUC)
        {
            List<SERVICE> service = Data.Ins.DB.SERVICEs.ToList();
            serviceSelectionUC.ServiceList.ItemsSource = service; 
        }    
        public void PreviousUC()
        {
            MainVM.PreviousUC();
        }
    }
}
