using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;


namespace WeddingApp.ViewModels
{
    class ServiceSelectionUC
    {

        public ICommand LoadedCommand { get; set; }
        public ICommand DeleteCartCommand { get; set; }
        public ICommand DeleteIsCheckedCartCommand { get; set; }

        public ICommand AllCheckedCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public ICommand OrderCommand { get; set; }

        private List<SERVICE> currentService;

        public List<SERVICE> CurrentService
        {
            get => currentService;
            set
            {
                currentService = value;
                OnPropertyChanged("currentService");
            }
        }


        public ServiceSelectionUC()
        {
            OrderCommand = new RelayCommand<ServiceSelectionUC>((parameter) => { return true; }, (parameter) => Order(parameter));
            LoadedCommand = new RelayCommand<ServiceSelectionUC>(p => p == null ? false : true, p => Loaded(p));
            DeleteCartCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => DeleteCart(parameter));
            DeleteIsCheckedCartCommand = new RelayCommand<ListView>((parameter) => { return true; }, (parameter) => DeleteIsCheckedCart(parameter));
            AllCheckedCommand = new RelayCommand<ServiceSelectionUC>((parameter) => { return true; }, (parameter) => AllChecked(parameter));
            CheckedCommand = new RelayCommand<CheckBox>((parameter) => { return true; }, (parameter) => Checked(parameter));


        }

        private void Loaded(ServiceSelectionUC cartUC)
        {
            CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();
            var user = Data.Ins.DB.USERS.Where(x => x.USERNAME_ == CurrentAccount.Username).SingleOrDefault();
            Name = user.FULLNAME_;
            Phone = user.PHONE_;
            Mail = user.EMAIL_;
            Address = user.ADDRESS_;

            //Ẩn địa chỉ nếu chưa có
            if (!String.IsNullOrEmpty(Address))
            {
                cartUC.SetAddress.Visibility = Visibility.Collapsed;
            }

            FoodCount = GetFoodCount(cartUC.cartList);
        }

        protected void DeleteCart(ListViewItem parameter)
        {
            try
            {
                if (CustomMessageBox.Show("Xóa món ăn khỏi giỏ hàng?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    CART cartToDelete = parameter.DataContext as CART;
                    Data.Ins.DB.CARTs.Remove(cartToDelete);
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Xóa thành công", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    var lv = GetAncestorOfType<ListView>(parameter);
                    var cartUC = GetAncestorOfType<CartUC>(parameter);

                    //Cập nhật lại checkbox
                    if (cartUC.selectAllCheckBox.IsChecked == true)
                    {
                        cartUC.selectAllCheckBox.IsChecked = false;
                    }

                    //Cập nhật lại giỏ hàng
                    CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();
                    TotalPrice = GetTotalPrice(lv);
                    FoodCount = GetFoodCount(lv);
                }
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected void DeleteIsCheckedCart(ListView parameter)
        {
            if (parameter.Items.Count == 0 || FoodCount == 0)
            {
                return;
            }

            try
            {
                if (CustomMessageBox.Show("Xóa các món ăn đang được chọn khỏi giỏ hàng?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    bool isAllChecked = true;
                    foreach (var lvi in FindVisualChildren<ListViewItem>(parameter))
                    {
                        CART cart = lvi.DataContext as CART;
                        var checkBox = GetVisualChild<CheckBox>(lvi);
                        //Chỉ xóa các món ăn được check
                        if (checkBox.IsChecked == true)
                        {
                            Data.Ins.DB.CARTs.Remove(cart);
                        }
                        else
                        {
                            isAllChecked = false;
                        }
                        if (isAllChecked)
                        {
                            var wd = GetAncestorOfType<CartUC>(parameter);
                            wd.selectAllCheckBox.IsChecked = false;
                        }
                    }
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Xóa thành công", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();
                }
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            TotalPrice = GetTotalPrice(parameter);
            FoodCount = GetFoodCount(parameter);
        }

        private void AllChecked(ServiceSelectionUC parameter)
        {
            bool newVal = (parameter.selectAllCheckBox.IsChecked == true);
            //true nếu isChecked == true và ngược lại

            //Set lại các checkbox giống với trạng thái của AllCheckBox
            foreach (var item in FindVisualChildren<CheckBox>(parameter.cartList))
            {
                item.IsChecked = newVal;
            }
            TotalPrice = GetTotalPrice(parameter.cartList);
            FoodCount = GetFoodCount(parameter.cartList);
        }

        private void Checked(CheckBox parameter)
        {
            var lv = GetAncestorOfType<ListView>(parameter);

            TotalPrice = GetTotalPrice(lv);
            FoodCount = GetFoodCount(lv);

            // Check xem nếu checked hết thì check cái ô trên cùng
            bool isAllChecked = true;
            var cartUC = GetAncestorOfType<CartUC>(lv);
            foreach (var item in FindVisualChildren<CheckBox>(lv))
            {
                if (item.IsChecked == false)
                {
                    cartUC.selectAllCheckBox.IsChecked = false;
                    isAllChecked = false;
                    break;
                }
            }
            if (isAllChecked)
            {
                cartUC.selectAllCheckBox.IsChecked = true;
            }
        }

        private int GetFoodCount(ListView listView)
        {
            int res = 0;
            foreach (var lvi in FindVisualChildren<ListViewItem>(listView))
            {
                CART cart = lvi.DataContext as CART;
                var checkBox = GetVisualChild<CheckBox>(lvi);
                if (checkBox.IsChecked == true)
                {
                    res++;
                }
            }
            return res;
        }

        // Tính tổng giá của thằng item có checked = true

        private long GetTotalPrice(ListView listView)
        {
            long res = 0;
            foreach (var lvi in FindVisualChildren<ListViewItem>(listView))
            {
                CART cart = lvi.DataContext as CART;
                var checkBox = GetVisualChild<CheckBox>(lvi);
                if (checkBox.IsChecked == true)
                {
                    // ép kiểu Giá = số lượng * giá sản phẩm * discount
                    res += (long)((Int32)cart.AMOUNT_ * (Int32)cart.PRODUCT.PRICE_ * (1 - (decimal)cart.PRODUCT.DISCOUNT_));
                }
            }
            return res;
        }

        private CultureInfo viVn = new CultureInfo("vi-VN");

        public void Order(CartUC parameter)
        {
            try
            {
                //Kiểm tra thông tin
                if (FoodCount == 0)
                {
                    CustomMessageBox.Show("Chưa chọn món ăn!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (String.IsNullOrEmpty(Address))
                {
                    CustomMessageBox.Show("Chưa thiết lập địa chỉ!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                DateTime Now = DateTime.Now;
                string now = Now.GetDateTimeFormats(viVn)[0];

                HashSet<RECEIPT_DETAIL> receipt_detail = new HashSet<RECEIPT_DETAIL>();

                //Tạo thông tin đơn hàng
                int countReceipt = Data.Ins.DB.RECEIPTs.Count() + 1;
                RECEIPT receipt = new RECEIPT();
                receipt.ID_ = countReceipt.ToString();
                receipt.STATUS_ = "0";
                receipt.DATE_ = Now;
                receipt.USERNAME_ = CurrentAccount.Username;
                receipt.VALUE_ = (int)TotalPrice;

                USER user = Data.Ins.DB.USERS.Where(user1 => user1.USERNAME_ == CurrentAccount.Username).Single();
                receipt.USER = user;

                int countReceiptDetail = Data.Ins.DB.RECEIPT_DETAIL.Count() + 1;

                foreach (var lvi in FindVisualChildren<ListViewItem>(parameter.cartList))
                {
                    CART cart = lvi.DataContext as CART;
                    var checkBox = GetVisualChild<CheckBox>(lvi);
                    if (checkBox.IsChecked == true)
                    {
                        receipt_detail.Add(new RECEIPT_DETAIL()
                        {
                            DETAIL_ID = countReceiptDetail.ToString(),
                            AMOUNT_ = (short)cart.AMOUNT_,
                            RECEIPT_ID = receipt.ID_,
                            PRODUCT_ = cart.PRODUCT_,
                            PRODUCT = cart.PRODUCT,
                            RATED_ = true,
                            RATING_ = 0,
                            RECEIPT = receipt
                        });
                        countReceiptDetail++;
                    }
                }

                foreach (var receipt_de in receipt_detail)
                {
                    Data.Ins.DB.RECEIPT_DETAIL.Add(receipt_de);
                }
                receipt.RECEIPT_DETAIL = receipt_detail;

                foreach (var lvi in FindVisualChildren<ListViewItem>(parameter.cartList))
                {
                    CART cart = lvi.DataContext as CART;
                    var checkBox = GetVisualChild<CheckBox>(lvi);
                    if (checkBox.IsChecked == true)
                    {
                        Data.Ins.DB.CARTs.Remove(cart);
                    }
                }
                Data.Ins.DB.SaveChanges();
                CurrentCart = Data.Ins.DB.CARTs.Where(cart => cart.USERNAME_ == CurrentAccount.Username).ToList();

                //reset giá trị về mặc định
                parameter.selectAllCheckBox.IsChecked = false;
                TotalPrice = 0;
                FoodCount = 0;

                CustomMessageBox.Show("Đơn hàng đã được tạo thành công đang chờ xử lí...", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch
            {
                CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





    }
}
