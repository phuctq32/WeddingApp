using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Views.UserControls;
using WeddingApp.Models;
using System.Windows.Controls;

namespace WeddingApp.ViewModels
{
    internal class InvoiceListVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand PayCommand  { get; set; }
        private List<INVOICE> listInvoice;

        public List<INVOICE> ListInvoice
        {
            get => listInvoice;
            set
            {
                listInvoice = value;
                OnPropertyChanged("Status");
                OnPropertyChanged("ListInvoice");
            }
        }

        public InvoiceListVM()
        {
            LoadedCommand = new RelayCommand<InvoiceListUC>(parameter => true, parameter => Loaded(parameter));
            PayCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Paid(parameter));

        }
        public void Loaded(InvoiceListUC invoiceListUC)
        {
            ListInvoice = Data.Ins.DB.INVOICES.Where(x=>x.STATUS == 1).ToList();
        }
        public void Paid(ListViewItem listViewItem)
        {
            INVOICE paidInvoice = listViewItem.DataContext as INVOICE;
            paidInvoice.STATUS = 2;
            Data.Ins.DB.SaveChanges();
            ListInvoice.Clear();
        }
    }
}
