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
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace WeddingApp.ViewModels
{
    internal class MenuVM :ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand PreviousUCCommand { get; set; }
        public ICommand NextUCCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand HoverItemCommand { get; set; }
        public ICommand CancelHoverItemCommand { get; set; }
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
            SearchCommand = new RelayCommand<MenuUC>(p => true, p => Search(p));
            HoverItemCommand = new RelayCommand<Button>((paramter) => paramter == null ? false : true, (parameter) => HoverItem(parameter));
            CancelHoverItemCommand = new RelayCommand<Button>((paramter) => paramter == null ? false : true, (parameter) => CancelHoverItem(parameter));
        }
        public void Loaded(MenuUC menuUC)
        {
            List<DISH> dishList = Data.Ins.DB.DISHES.ToList();
            menuUC.ViewListProducts.ItemsSource = dishList;
            thisUC = menuUC;
            menuUC.combox.SelectionChanged += new SelectionChangedEventHandler(SelectionChanged);
        }
        public void SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            List<DISH> SortedDish;
            switch (thisUC.combox.SelectedIndex)
            {
                case 0:
                    SortedDish = Data.Ins.DB.DISHES.OrderBy(p => p.COST).ToList();
                    thisUC.ViewListProducts.ItemsSource = SortedDish;
                    break;
                case 1:
                    SortedDish = Data.Ins.DB.DISHES.OrderByDescending(p => p.COST).ToList();
                    thisUC.ViewListProducts.ItemsSource = SortedDish;
                    break;
            }
        }
        public void Search(MenuUC menuUC)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(menuUC.ViewListProducts.ItemsSource);
            view.Filter = CompareString;
            CollectionViewSource.GetDefaultView(menuUC.ViewListProducts.ItemsSource).Refresh();
        }

        private bool CompareString(object item)
        {
            string a = (item as DISH).DISHNAME;
            string Search = thisUC.SearchTxt.Text.Trim();
            string b = Search;
            a = RemoveSign4VietnameseString(a);
            if (b != null)
            {
                b = RemoveSign4VietnameseString(b);
            }
            if (string.IsNullOrEmpty(b))
                return true;
            else
                return (a.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };
        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
        public bool isSearched(string a, string b)
        {
            if (string.CompareOrdinal(a, b) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void AddToCart(ListViewItem listViewItem)
        {
            DISH selectedDish = listViewItem.DataContext as DISH;
            if (!thisUC.carts.Items.Contains(selectedDish))
            {
                thisUC.carts.Items.Add(selectedDish);
            }
            TotalPrice = GetTotalPrice(thisUC.carts);
        }
        public void PreviousUC(MenuUC menuUC)
        {
            MainVM.PreviousUC();
        }
        public void NextUC(MenuUC menuUC)
        {
            int minimumCost = Convert.ToInt32(Data.Ins.DB.BALLROOMs.Where(x => x.BALLROOMNAME == MainVM.WeddingHall).SingleOrDefault().BALLROOMTYPE.MINIMUMCOST);
            if (menuUC.carts.Items.Count < 5)
            {
                CustomMessageBox.Show("Vui lòng chọn tối thiểu 5 món", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            else if(totalPrice < minimumCost)
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo("vi-VN");
                CustomMessageBox.Show("Vui lòng chọn đơn giá tối thiểu " + minimumCost.ToString("C0", cultureInfo), System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            else
            {
                MainVM.NextUC();
            }
        }
        private long GetTotalPrice(ListView listView)
        {
            long res = 0;
            foreach (DISH Cart in listView.Items)
            {
                res += Convert.ToInt32(Cart.COST);
            }
            return res;
        }
        private void HoverItem(Button deleteBtn)
        {
            deleteBtn.Visibility = Visibility.Visible;
        }

        private void CancelHoverItem(Button deleteBtn)
        {
            deleteBtn.Visibility = Visibility.Collapsed;
        }
    }
}
