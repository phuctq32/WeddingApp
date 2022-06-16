using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using WeddingApp.Models;
using WeddingApp.Views.UserControls.Admin;
using WeddingApp.Views;
using System.Windows.Controls;
using System.Globalization;

namespace WeddingApp.ViewModels
{
    internal class CompleteInvoiceVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand DetailCommand { get; set; }
        public CompleteInvoiceVM()
        {
            LoadedCommand = new RelayCommand<CompletedInvoiceListUC>(p => true, p => Loaded(p));
            DetailCommand = new RelayCommand<ListViewItem>(p => true, p => Detail(p));
        }
        public void Loaded(CompletedInvoiceListUC completedInvoiceListUC)
        {
            List<INVOICE> completeInvoice = Data.Ins.DB.INVOICES.Where(x => x.STATUS == 2).ToList();
            completedInvoiceListUC.listViewInvoice.ItemsSource = completeInvoice;
        }
        public void Detail(ListViewItem listViewItem)
        {
            CompletedInvoiceWindow completedInvoiceWindow = new CompletedInvoiceWindow();
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("vi-VN");
            INVOICE paidInvoice = listViewItem.DataContext as INVOICE;

            completedInvoiceWindow.txtWeddingID.Text = paidInvoice.WEDDINGID.ToString();
            completedInvoiceWindow.txtGroom.Text = paidInvoice.WEDDING.GROOMNAME;
            completedInvoiceWindow.txtBride.Text = paidInvoice.WEDDING.BRIDENAME;
            completedInvoiceWindow.txtDeposit.Text = paidInvoice.WEDDING.DEPOSIT.ToString("C0", cultureInfo);
            completedInvoiceWindow.txtRemaining.Text = paidInvoice.REMAININGCOST.ToString("C0", cultureInfo);
            completedInvoiceWindow.txtTableAmount.Text = paidInvoice.WEDDING.TABLEAMOUNT.ToString();
            completedInvoiceWindow.txtTableCost.Text = Convert.ToInt32(paidInvoice.TABLECOST).ToString("C0", cultureInfo);
            completedInvoiceWindow.txtTotalCost.Text = paidInvoice.TOTALCOST.ToString("C0", cultureInfo);
            completedInvoiceWindow.txtTotalServiceCost.Text = paidInvoice.SERVICECOST.ToString("C0", cultureInfo);
            completedInvoiceWindow.txtWeddingCost.Text = paidInvoice.WEDDINGCOST.ToString("C0", cultureInfo);
            completedInvoiceWindow.txtWeddingDate.Text = paidInvoice.WEDDING.WEDDINGDATE.ToString("dd/MM/yyyy");
            completedInvoiceWindow.txtPenalty.Text = Convert.ToInt32(paidInvoice.PENALTY).ToString("C0", cultureInfo);
            List<USEDSERVICE> sERVEs = Data.Ins.DB.USEDSERVICES.Where(x => x.WEDDINGID == paidInvoice.WEDDINGID).ToList();
            completedInvoiceWindow.listView.ItemsSource = sERVEs;
            completedInvoiceWindow.Show();
        }
    }
}
