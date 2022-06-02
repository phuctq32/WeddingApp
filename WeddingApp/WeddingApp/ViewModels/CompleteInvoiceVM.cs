using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using WeddingApp.Models;
using WeddingApp.Views.UserControls.Admin;
using WeddingApp.Views;

namespace WeddingApp.ViewModels
{
    internal class CompleteInvoiceVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand DetailCommand { get; set; }
        public CompleteInvoiceVM()
        {
            LoadedCommand = new RelayCommand<CompletedInvoiceListUC>(p => true, p => Loaded(p));
            DetailCommand = new RelayCommand<CompletedInvoiceListUC>(p => true, p => Detail(p));
        }
        public void Loaded(CompletedInvoiceListUC completedInvoiceListUC)
        {
            List<INVOICE> completeInvoice = Data.Ins.DB.INVOICES.Where(x => x.STATUS == 2).ToList();
            completedInvoiceListUC.listViewInvoice.ItemsSource = completeInvoice;
        }
        public void Detail(CompletedInvoiceListUC completedInvoiceListUC)
        {
            WeddingDetailWindow weddingDetailWindow = new WeddingDetailWindow();
        }
    }
}
