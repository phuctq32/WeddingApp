using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using WeddingApp.Views;
using WeddingApp.Views.UserControls;
using WeddingApp.Models;

namespace WeddingApp.ViewModels
{
    internal class WeddingDetailVM : ViewModelBase
    {
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public WeddingDetailVM()
        {
            LoadedCommand = new RelayCommand<WeddingDetailWindow>(p => true, p => Loaded(p));
            SelectionChangedCommand = new RelayCommand<WeddingDetailWindow>(p => true, p => SwitchTab(p));
        }
        public void Loaded(WeddingDetailWindow weddingDetailWindow)
        {
            
        }
        public void SwitchTab(WeddingDetailWindow weddingDetailWindow)
        {
            int WeddingID = Convert.ToInt32(weddingDetailWindow.txtWeddingID.Text);
            switch (weddingDetailWindow.listViewTab.SelectedIndex)
            {
                case 0:
                    DetailDishListUC detailDishListUC = new DetailDishListUC();
                    List<MENU> Menu = Data.Ins.DB.MENUS.Where(x=>x.WEDDINGID == WeddingID).ToList();
                    detailDishListUC.ListDishUse.ItemsSource = Menu;
                    weddingDetailWindow.listViewdish.Children.Clear();
                    weddingDetailWindow.listViewdish.Children.Add(detailDishListUC);
                    break;
                case 1:
                    DetailServiceListUC detailServiceListUC = new DetailServiceListUC();
                    List<USEDSERVICE> Serve = Data.Ins.DB.USEDSERVICES.Where(x => x.WEDDINGID == WeddingID).ToList();
                    detailServiceListUC.ListServiceUse.ItemsSource = Serve;
                    weddingDetailWindow.listViewdish.Children.Clear();
                    weddingDetailWindow.listViewdish.Children.Add(detailServiceListUC);
                    break;
            }
        }
    }
}
