using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingApp.Models;
using WeddingApp.Views.UserControls;
using WeddingApp.Views;
using System.Windows.Input;
using System.Windows.Controls;
using System.Globalization;

namespace WeddingApp.ViewModels
{
    internal class WeddingListVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand OpenWeddingCommand { get; set; }   

        public WeddingListVM()
        {
            LoadedCommand = new RelayCommand<WeddingListUC>(parameter => true, parameter => Load(parameter));
            OpenWeddingCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Open(parameter));
        }
        public void Load(WeddingListUC weddingListUC)
        {
            List<WEDDING> wEDDINGs = new List<WEDDING>();
            wEDDINGs = Data.Ins.DB.WEDDINGS.ToList();
            weddingListUC.weddingList.ItemsSource = wEDDINGs;
        }
        public void Open(ListViewItem listViewItem)
        {
            WEDDING selectedWedding = listViewItem.DataContext as WEDDING;
            INVOICE iNVOICE = Data.Ins.DB.INVOICES.Where(x => x.WEDDINGID == selectedWedding.WEDDINGID).SingleOrDefault();
            List<MENU> menu = Data.Ins.DB.MENUS.Where(x => x.WEDDINGID == selectedWedding.WEDDINGID).ToList();
            WeddingDetailWindow weddingDetailWindow = new WeddingDetailWindow();
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("vi-VN");
            weddingDetailWindow.txtWeddingID.Text = selectedWedding.WEDDINGID.ToString();
            weddingDetailWindow.txtGroom.Text = selectedWedding.GROOMNAME;
            weddingDetailWindow.txtBride.Text = selectedWedding.BRIDENAME;
            weddingDetailWindow.txtPhone.Text = selectedWedding.TELEPHONE;
            weddingDetailWindow.txtDeposit.Text = selectedWedding.DEPOSIT.ToString("C0", cultureInfo);
            weddingDetailWindow.txtWeddingDate.Text = selectedWedding.WEDDINGDATE.ToString("dd/MM/yyyy");
            weddingDetailWindow.txtTableAmount.Text = selectedWedding.TABLEAMOUNT.ToString();
            weddingDetailWindow.txtShift.Text = selectedWedding.SHIFT.STARTTIME.ToString();
            weddingDetailWindow.txtReserve.Text = selectedWedding.RESERVEAMOUNT.ToString();
            weddingDetailWindow.txtHall.Text = selectedWedding.BALLROOM.BALLROOMNAME.ToString();
            weddingDetailWindow.txtDeposit2.Text = selectedWedding.DEPOSIT.ToString("C0", cultureInfo);
            weddingDetailWindow.txtTotalService.Text = iNVOICE.SERVICECOST.ToString("C0", cultureInfo);
            weddingDetailWindow.txtTotal.Text = iNVOICE.TOTALCOST.ToString("C0", cultureInfo);
            weddingDetailWindow.txtRemaining.Text = iNVOICE.REMAININGCOST.ToString("C0", cultureInfo);
            DetailDishListUC detailDishListUC = new DetailDishListUC();
            detailDishListUC.ListDishUse.ItemsSource = menu;
            weddingDetailWindow.listViewdish.Children.Clear();
            weddingDetailWindow.listViewdish.Children.Add(detailDishListUC);
            weddingDetailWindow.ShowDialog();
        }
    }
}
