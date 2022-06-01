using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views.UserControls;
using WeddingApp.ViewModels;

namespace WeddingApp.ViewModels
{
    internal class ServiceVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand PreviousUCCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public ICommand AllCheckedCommand { get; set; }
        public ICommand DownCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand ConfirmCommand  { get; set; }
        public ServiceSelectionUC thisUC;
        private long totalPrice;
        private List<SERVE> Listserve;
        private WEDDING newWedding = new WEDDING();
        public long TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }
        public ServiceVM()
        {
            LoadedCommand = new RelayCommand<ServiceSelectionUC>(p => true, p => Loaded(p));
            PreviousUCCommand = new RelayCommand<ServiceSelectionUC>(p => true, p => PreviousUC());
            DownCommand = new RelayCommand<TextBlock>(p => true, p => Down(p));
            UpCommand = new RelayCommand<TextBlock>(p => true, p => Up(p));
            CheckedCommand = new RelayCommand<CheckBox>((parameter) => { return true; }, (parameter) => Checked(parameter));
            AllCheckedCommand = new RelayCommand<ServiceSelectionUC>((parameter) => { return true; }, (parameter) => AllChecked(parameter));
            ConfirmCommand = new RelayCommand<ServiceSelectionUC>(p => true, p => Confirm(p));
            Listserve = new List<SERVE>();
        }
        public void Loaded(ServiceSelectionUC serviceSelectionUC)
        {
            List<SERVICE> service = Data.Ins.DB.SERVICEs.ToList();
            serviceSelectionUC.ServiceList.ItemsSource = service; 
            thisUC = serviceSelectionUC;
        }    
        public void PreviousUC()
        {
            MainVM.PreviousUC();
        }
        private void Down(TextBlock parameter)
        {
            short amount = short.Parse(parameter.Text.ToString());

            //Lấy <đối tượng> là cha của parameter bằng GetAncestorOfType
            
            var lvi = GetAncestorOfType<ListViewItem>(parameter);

            //Xét trường hợp xóa món ăn nếu giảm số lượng xuống 0

            //Giảm số lượng của sản phẩm
            SERVE cart = lvi.DataContext as SERVE;
            if (amount > 1)
            {
                amount--;
                cart.AMOUNT = Convert.ToByte(amount);
                parameter.Text = amount.ToString();
            }

            TotalPrice = GetTotalPrice(thisUC.carts);
        }

        private void Up(TextBlock parameter)
        {
            short amount = short.Parse(parameter.Text.ToString());
            if (amount == short.MaxValue)
            {
                //Không thay đổi khi maxValue
                return;
            }
            else
            {
                //Lấy <đối tượng> là cha của parameter bằng GetAncestorOfType
                var lvi = GetAncestorOfType<ListViewItem>(parameter);
                SERVE cart = lvi.DataContext as SERVE;

                //Tăng số lượng của sản phẩm
                amount++;
                cart.AMOUNT = Convert.ToByte(amount);
                parameter.Text = amount.ToString();
                TotalPrice = GetTotalPrice(thisUC.carts);
            }
        }
        private void AllChecked(ServiceSelectionUC parameter)
        {
            bool newVal = (parameter.selectAllCheckBox.IsChecked == true);
            //true nếu isChecked == true và ngược lại

            //Set lại các checkbox giống với trạng thái của AllCheckBox
            foreach (var item in FindVisualChildren<CheckBox>(parameter.ServiceList))
            {
                item.IsChecked = newVal;
            }
            TotalPrice = GetTotalPrice(parameter.carts);
            //FoodCount = GetFoodCount(parameter.cartList);
        }
        private void Checked(CheckBox parameter)
        {
            var lv = GetAncestorOfType<ListView>(parameter);
            var lv1 = GetAncestorOfType<ListViewItem>(parameter);
            //FoodCount = GetFoodCount(lv);

            // Check xem nếu checked hết thì check cái ô trên cùng
            bool isAllChecked = true;
            var serviceSelectionUC = GetAncestorOfType<ServiceSelectionUC>(lv);
            foreach (var item in FindVisualChildren<CheckBox>(lv))
            {
                if (item.IsChecked == false)
                {
                    serviceSelectionUC.selectAllCheckBox.IsChecked = false;
                    isAllChecked = false;
                    break;
                }
            }
            if (isAllChecked)
            {
                serviceSelectionUC.selectAllCheckBox.IsChecked = true;
            }
            SERVICE selectedService = lv1.DataContext as SERVICE;
            SERVE serve = new SERVE();
            serve.SERVICEID = selectedService.SERVICEID;
            serve.SERVICECOST = selectedService.COST;
            var lv2 = FindChild<TextBlock>(lv1, "amount");
            serve.AMOUNT = Convert.ToByte(lv2.Text);
            if (parameter.IsChecked == true)
            {
                Listserve.Add(serve);
                thisUC.carts.Items.Add(serve);
                TotalPrice = GetTotalPrice(thisUC.carts);
            }
            else
            {
                SERVE deleteServe = Listserve.Find(x => x.SERVICEID == serve.SERVICEID);
                Listserve.Remove(deleteServe);
                thisUC.carts.Items.Remove(deleteServe);
                TotalPrice = GetTotalPrice(thisUC.carts);
            }
        }
        private long GetTotalPrice(ListView listView)
        {
            long res = 0;
            foreach (SERVE lvi in listView.Items)
            {
                //SERVE cart = lvi.DataContext as SERVE;
                
                    // ép kiểu Giá = số lượng * giá sản phẩm * discount
                    res += (long)(Int32)lvi.AMOUNT * (long)(Int32)lvi.SERVICECOST;
                
            }
            return res;
        }
        public void Confirm(ServiceSelectionUC serviceSelectionUC)
        {
            if(CustomMessageBox.Show("Xác nhận đặt tiệc?", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
            {
                MenuUC menuUC = (MenuUC)MainVM.PreviousUCs.Pop();
                SetWeddingInfomationUC setWeddingInfomationUC = (SetWeddingInfomationUC)MainVM.PreviousUCs.Pop();
                WeddingInformationSave(setWeddingInfomationUC);
                InvoiceSave(serviceSelectionUC, menuUC);
                MenuSave(menuUC);
                ServeSave(serviceSelectionUC);
                CustomMessageBox.Show("Đặt tiệc thành công", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
            }
        }

        public void WeddingInformationSave(SetWeddingInfomationUC setWeddingInfomationUC)
        {
            newWedding.BALLROOMID = Data.Ins.DB.BALLROOMs.Where(x => x.BALLROOMNAME == setWeddingInfomationUC.hallComboBox.Text).SingleOrDefault().BALLROOMID;
            newWedding.BOOKINGDATE = DateTime.Now;
            newWedding.BRIDE = setWeddingInfomationUC.txtbride.Text;
            newWedding.GROOM = setWeddingInfomationUC.txtgroom.Text;
            newWedding.TELEPHONE = setWeddingInfomationUC.txtphone.Text;
            newWedding.RESERVEAMOUNT = Convert.ToByte(setWeddingInfomationUC.comboBoxreversedTableAmount.Text);
            newWedding.TABLEAMOUNT = Convert.ToByte(setWeddingInfomationUC.comboBoxTableAmount.Text);
            newWedding.WEDDINGDATE = DateTime.Parse(setWeddingInfomationUC.date.Text);
            newWedding.SHIFTID = 1; //Chưa làm comboBox
        }

        public void InvoiceSave(ServiceSelectionUC serviceSelectionUC, MenuUC menuUC)
        {
            INVOICE newInvoice = new INVOICE();
            newInvoice.WEDDINGCOST = Convert.ToInt32(menuUC.txtTotalprice.Text.Replace(",",""));
            newInvoice.SERVICECOST = Convert.ToInt32(serviceSelectionUC.totalPrice.Text.Replace(",", ""));
            newInvoice.STATUS = 0;
            newInvoice.TOTALCOST = newInvoice.WEDDINGCOST + newInvoice.SERVICECOST;
            newInvoice.REMAININGCOST = newInvoice.TOTALCOST * (decimal)0.9;
            newWedding.DEPOSIT = newInvoice.TOTALCOST * (decimal)0.1;
            Data.Ins.DB.WEDDINGs.Add(newWedding);
            Data.Ins.DB.SaveChanges();
            newInvoice.WEDDINGID = newWedding.WEDDINGID;
            newInvoice.USERNAME = CurrentAccount.Username;
            Data.Ins.DB.INVOICES.Add(newInvoice);
            Data.Ins.DB.SaveChanges();
        }
        public void MenuSave(MenuUC menuUC)
        {
            List<MENU> menuList = new List<MENU>(); 
            foreach(DISH item in menuUC.ViewListProducts.Items)
            {
                MENU menu = new MENU();
                menu.WEDDINGID = newWedding.WEDDINGID;
                menu.DISHID = item.DISHID;
                menu.DISHCOST = item.COST;
                Data.Ins.DB.MENUs.Add(menu);
                Data.Ins.DB.SaveChanges();
            }
        }
        public void ServeSave(ServiceSelectionUC serviceSelectionUC)
        {
            List<SERVE> ServiceList = new List<SERVE>();
            foreach(SERVE item in serviceSelectionUC.ServiceList.Items)
            {
                SERVE serve = new SERVE();
                serve.SERVICEID = item.SERVICEID;
                serve.WEDDINGID = newWedding.WEDDINGID;
                serve.SERVICECOST = item.SERVICECOST;
                serve.AMOUNT = item.AMOUNT;
                Data.Ins.DB.SERVEs.Add(serve);
                Data.Ins.DB.SaveChanges();
            }
        }

    }
}
