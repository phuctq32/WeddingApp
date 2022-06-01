using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Text;
using System.Threading.Tasks;
using WeddingApp.Models;
using WeddingApp.Views;
using System.Windows.Controls;
using System.Windows;
using System.Globalization;

namespace WeddingApp.ViewModels
{
    internal class InvoiceVM:ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }

        public ICommand PayButtonCommand { get; set; }
        public ICommand PenaltyCheckedCommand { get; set; }

        public InvoiceWindow thisWD;

        public PARAMETER isPenalty = Data.Ins.DB.PARAMETERs.Where(x => x.PARAMETERID == "PHAT").SingleOrDefault();
        public PARAMETER Penalty = Data.Ins.DB.PARAMETERs.Where(x => x.PARAMETERID == "MUCPHAT").SingleOrDefault();
        public INVOICE thisInvoice;
        CultureInfo cultureInfo = CultureInfo.GetCultureInfo("vi-VN");
        public InvoiceVM()
        {
            LoadedCommand = new RelayCommand<InvoiceWindow>(parameter => true, parameter => Loaded(parameter));
            PayButtonCommand = new RelayCommand<InvoiceWindow>(parameter => true, parameter => Pay(parameter));
            PenaltyCheckedCommand = new RelayCommand<InvoiceWindow>(parameter => true, parameter => Check(parameter));
        }
        public void Loaded(InvoiceWindow invoiceWindow)
        {
            thisWD = invoiceWindow;
            int WeddingID = Convert.ToInt32(invoiceWindow.txtWeddingID.Text);
            if (Penalty.PARAMETERVALUE == 1)
            {
                invoiceWindow.chkPenalty.IsChecked = true;
            }
            else
            {
                invoiceWindow.chkPenalty.IsChecked = false;
            }
            thisInvoice = Data.Ins.DB.INVOICES.Where(x => x.WEDDINGID == WeddingID).SingleOrDefault();
            double penalty;
            if (thisInvoice.WEDDING.WEDDINGDATE < DateTime.Now && isPenalty.PARAMETERVALUE == 1)
            {
                TimeSpan a = thisInvoice.WEDDING.WEDDINGDATE.Date - DateTime.Now.Date;
                penalty = Penalty.PARAMETERVALUE * Convert.ToInt32(thisInvoice.TOTALCOST) * a.Days;
            }
            else
            {
                penalty = 0;
            }
            invoiceWindow.txtPenalty.Text = penalty.ToString("C0", cultureInfo);
        }
        public void Pay(InvoiceWindow invoiceWindow)
        {
            if(CustomMessageBox.Show("Thanh toán hóa đơn hiện tại?", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
            {
                thisInvoice.STATUS = 2;
                thisInvoice.PENALTIES = Convert.ToDecimal(invoiceWindow.txtPenalty.Text);
                Data.Ins.DB.SaveChanges();
                CustomMessageBox.Show("Thanh toán thành công", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
        }
        public void Check(InvoiceWindow invoiceWindow)
        {
            if (Penalty.PARAMETERVALUE == 1)
            {
                thisWD.chkPenalty.IsChecked = false;
                Penalty.PARAMETERVALUE = 0;
                Data.Ins.DB.SaveChanges();
            }
            else
            {
                thisWD.chkPenalty.IsChecked = true;
                Penalty.PARAMETERVALUE = 1;
                Data.Ins.DB.SaveChanges();
            }
        }
    }
}
