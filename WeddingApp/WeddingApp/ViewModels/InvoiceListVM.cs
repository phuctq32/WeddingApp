using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Views.UserControls;
using WeddingApp.Models;
using WeddingApp.Views;
using System.Windows.Controls;
using System.Globalization;

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
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("vi-VN");
            INVOICE paidInvoice = listViewItem.DataContext as INVOICE;
            InvoiceWindow invoiceWindow = new InvoiceWindow();
            
            invoiceWindow.txtWeddingID.Text = paidInvoice.WEDDINGID.ToString();
            invoiceWindow.txtGroom.Text = paidInvoice.WEDDING.GROOM;
            invoiceWindow.txtBride.Text = paidInvoice.WEDDING.BRIDE;
            invoiceWindow.txtDeposit.Text = paidInvoice.WEDDING.DEPOSIT.ToString("C0", cultureInfo);
            invoiceWindow.txtRemaining.Text = paidInvoice.REMAININGCOST.ToString("C0", cultureInfo);
            invoiceWindow.txtTableAmount.Text = paidInvoice.WEDDING.TABLEAMOUNT.ToString();
            invoiceWindow.txtTableCost.Text = Convert.ToInt32(paidInvoice.TABLECOST).ToString("C0", cultureInfo);
            invoiceWindow.txtTotalCost.Text = paidInvoice.TOTALCOST.ToString("C0", cultureInfo);
            invoiceWindow.txtTotalServiceCost.Text = paidInvoice.SERVICECOST.ToString("C0", cultureInfo);
            invoiceWindow.txtWeddingCost.Text = paidInvoice.WEDDINGCOST.ToString("C0", cultureInfo);
            invoiceWindow.txtWeddingDate.Text = paidInvoice.WEDDING.WEDDINGDATE.ToString("dd/MM/yyyy");
            invoiceWindow.ShowDialog();
            ListInvoice.Clear();
        }
    }
}
