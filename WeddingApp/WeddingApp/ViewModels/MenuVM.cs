using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using WeddingApp.Models;
using WeddingApp.Views.UserControls;
using WeddingApp.Views;
using System.Windows.Controls;

namespace WeddingApp.ViewModels
{
    internal class MenuVM :ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand PreviousUCCommand { get; set; }
        public ICommand NextUCCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        private long totalPrice;
        public long TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }
        public MenuUC thisUC;

        public MenuVM()
        {
            LoadedCommand = new RelayCommand<MenuUC>(p => true, p => Loaded(p));
            PreviousUCCommand = new RelayCommand<MenuUC>(p => true, p => PreviousUC(p));
            NextUCCommand = new RelayCommand<MenuUC>(p => true, p => NextUC(p));
            AddToCartCommand = new RelayCommand<ListViewItem>(p => true, p => AddToCart(p));
        }
        public void Loaded(MenuUC menuUC)
        {
            List<DISH> dishList = Data.Ins.DB.DISHES.ToList();
            menuUC.ViewListProducts.ItemsSource = dishList;
            thisUC = menuUC;
        }
        public void AddToCart(ListViewItem listViewItem)
        {
            DISH selectedDish = listViewItem.DataContext as DISH;
            thisUC.carts.Items.Add(selectedDish);
        }
        public void PreviousUC(MenuUC menuUC)
        {
            MainVM.PreviousUC();
        }
        public void NextUC(MenuUC menuUC)
        {
            MainVM.NextUC();
        }
        //private long GetTotalPrice(ListView listView)
        //{
        //    long res = 0;
        //    foreach (var lvi in FindVisualChildren<ListViewItem>(listView))
        //    {
        //        CART cart = lvi.DataContext as CART;
        //        var checkBox = GetVisualChild<CheckBox>(lvi);
        //        if (checkBox.IsChecked == true)
        //        {
        //            ép kiểu Giá = số lượng* giá sản phẩm *discount
        //            res += (long)((Int32)cart.AMOUNT_ * (Int32)cart.PRODUCT.PRICE_ * (1 - (decimal)cart.PRODUCT.DISCOUNT_));
        //        }
        //    }
        //    return res;
        //}
    }
}
