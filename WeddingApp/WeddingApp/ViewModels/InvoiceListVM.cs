using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Views.UserControls;
using WeddingApp.Models;

namespace WeddingApp.ViewModels
{
    internal class InvoiceListVM
    {
        public ICommand LoadedCommand { get; set; }

        public InvoiceListVM()
        {
            LoadedCommand = new RelayCommand<InvoiceListUC>(parameter => true, parameter => Loaded(parameter));
        }
        public void Loaded(InvoiceListUC invoiceListUC)
        {
            List<INVOICE> iNVOICEs = new List<INVOICE>();
            iNVOICEs = Data.Ins.DB.INVOICES.ToList();
            invoiceListUC.invoiceList.ItemsSource = iNVOICEs; 
        }


    }
}
