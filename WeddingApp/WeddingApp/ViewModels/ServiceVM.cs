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
using System.Globalization;

namespace WeddingApp.ViewModels
{
    internal class ServiceVM : ViewModelBase
    {
        private long totalPrice;
        private List<USEDSERVICE> serveList;
        private WEDDING newWedding = new WEDDING();

        public ICommand LoadedCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public ICommand AllCheckedCommand { get; set; }
        public ICommand DownCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand NextUCCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand DeleteCartCommand { get; set; }
        public ServiceSelectionUC thisUC;
        public long TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }
        public List<USEDSERVICE> ServeList
        {
            get => serveList;
            set
            {
                serveList = value;
                OnPropertyChanged(nameof(ServeList));
            }
        }
        public ServiceVM()
        {
            LoadedCommand = new RelayCommand<ServiceSelectionUC>(p => true, p => Loaded(p));
            DownCommand = new RelayCommand<TextBlock>(p => true, p => Down(p));
            UpCommand = new RelayCommand<TextBlock>(p => true, p => Up(p));
            CheckedCommand = new RelayCommand<CheckBox>((parameter) => { return true; }, (parameter) => Checked(parameter));
            AllCheckedCommand = new RelayCommand<ServiceSelectionUC>((parameter) => { return true; }, (parameter) => AllChecked(parameter));
            ConfirmCommand = new RelayCommand<ServiceSelectionUC>(p => true, p => Confirm(p));
            DeleteCartCommand = new RelayCommand<ListViewItem>(p => p == null ? false : true, (p) => DeleteCart(p));
            NextUCCommand = new RelayCommand<MenuUC>(p => true, p => NextUC(p));
            ServeList = new List<USEDSERVICE>();
        }
        public void Loaded(ServiceSelectionUC serviceSelectionUC)
        {
            thisUC = serviceSelectionUC;
            List<SERVICE> service = Data.Ins.DB.SERVICES.Where(x=>x.STATUS == 1).ToList();
            serviceSelectionUC.ServiceList.ItemsSource = service;
            List<USEDSERVICE> temp = new List<USEDSERVICE>();
            ServeList.ForEach(item => temp.Add(item));
            temp.Clear();
            ServeList = temp;
            TotalPrice = GetTotalPrice(ServeList);
        }
        public void NextUC(MenuUC menuUC)
        {
            int minimumCost = Convert.ToInt32(Data.Ins.DB.BALLROOMS.Where(x => x.BALLROOMNAME == MainVM.WeddingHall).SingleOrDefault().BALLROOMTYPE.MINIMUMCOST);
            long menuPrice = Convert.ToInt32(menuUC.txtTotalprice.Text.Replace(",", ""));
            if (menuUC.carts.Items.Count < 5)
            {
                CustomMessageBox.Show("Vui lòng chọn tối thiểu 5 món", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            else if (menuPrice < minimumCost)
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo("vi-VN");
                CustomMessageBox.Show("Vui lòng chọn đơn giá tối thiểu " + minimumCost.ToString("C0", cultureInfo), System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            else
            {
                MainVM.NextUC();
                if (thisUC != null)
                {
                    List<USEDSERVICE> temp = new List<USEDSERVICE>();
                    ServeList.ForEach(serve => temp.Add(serve));
                    ServeList = temp;
                    foreach (var checkBox in FindVisualChildren<CheckBox>(thisUC.ServiceList))
                    {
                        var lvi = GetAncestorOfType<ListViewItem>(checkBox);
                        SERVICE service = lvi.DataContext as SERVICE;
                        foreach (var item in ServeList)
                        {
                            if (item.SERVICEID == service.SERVICEID)
                            {
                                checkBox.IsChecked = true;
                            }
                        }
                    }
                    bool isALlChecked = true;
                    foreach (var item in FindVisualChildren<CheckBox>(thisUC.ServiceList))
                    {
                        if (item.IsChecked == false)
                        {
                            isALlChecked = false;
                            break;
                        }
                    }
                    thisUC.selectAllCheckBox.IsChecked = isALlChecked;
                }

            }
        }
        private void Down(TextBlock parameter)
        {
            short amount = short.Parse(parameter.Text.ToString());

            //Lấy <đối tượng> là cha của parameter bằng GetAncestorOfType

            var lvi = GetAncestorOfType<ListViewItem>(parameter);

            //Xét trường hợp xóa món ăn nếu giảm số lượng xuống 0

            //Giảm số lượng của sản phẩm
            USEDSERVICE cart = lvi.DataContext as USEDSERVICE;
            if (amount > 1)
            {
                amount--;
                cart.AMOUNT = Convert.ToByte(amount);
                cart.TOTALCOST = cart.AMOUNT * cart.SERVICECOST;
                parameter.Text = amount.ToString();
            }

            TotalPrice = GetTotalPrice(ServeList);
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
                USEDSERVICE cart = lvi.DataContext as USEDSERVICE;

                //Tăng số lượng của sản phẩm
                amount++;
                cart.AMOUNT = Convert.ToByte(amount);
                cart.TOTALCOST = cart.AMOUNT * cart.SERVICECOST;
                parameter.Text = amount.ToString();
                TotalPrice = GetTotalPrice(ServeList);
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
            if (newVal)
            {
                List<USEDSERVICE> temp = new List<USEDSERVICE>();
                foreach (SERVICE item in parameter.ServiceList.Items)
                {
                    USEDSERVICE serve = new USEDSERVICE();
                    serve.SERVICEID = item.SERVICEID;
                    serve.SERVICECOST = item.SERVICECOST;
                    serve.AMOUNT = 1;
                    serve.TOTALCOST = serve.SERVICECOST * serve.AMOUNT;
                    serve.SERVICE = Data.Ins.DB.SERVICES.Where(s => s.SERVICEID == serve.SERVICEID).SingleOrDefault();
                    temp.Add(serve);
                }
                ServeList = temp;
            }
            else
            {
                List<USEDSERVICE> temp = new List<USEDSERVICE>();
                temp.Clear();
                ServeList = temp;
            }
            TotalPrice = GetTotalPrice(ServeList);
        }
        private void Checked(CheckBox parameter)
        {
            var lv = GetAncestorOfType<ListView>(parameter);
            var lvi = GetAncestorOfType<ListViewItem>(parameter);

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
            SERVICE selectedService = lvi.DataContext as SERVICE;
            USEDSERVICE serve = new USEDSERVICE();
            serve.SERVICEID = selectedService.SERVICEID;
            serve.SERVICECOST = selectedService.SERVICECOST;
            serve.AMOUNT = 1;
            serve.TOTALCOST = serve.SERVICECOST * serve.AMOUNT;
            serve.SERVICE = Data.Ins.DB.SERVICES.Where(s => s.SERVICEID == serve.SERVICEID).SingleOrDefault();
            if (parameter.IsChecked == true)
            {
                List<USEDSERVICE> temp = new List<USEDSERVICE>();
                ServeList.ForEach(item => temp.Add(item));
                temp.Add(serve);
                ServeList = temp;
            }
            else
            {
                USEDSERVICE serveToDelete = ServeList.Find(x => x.SERVICEID == serve.SERVICEID);
                List<USEDSERVICE> temp = new List<USEDSERVICE>();
                ServeList.ForEach(item => temp.Add(item));
                temp.Remove(serveToDelete);
                ServeList = temp;
            }
            TotalPrice = GetTotalPrice(ServeList);
        }
        private long GetTotalPrice(List<USEDSERVICE> serves)
        {
            long res = 0;
            foreach (var item in serves)
            {
                res += (long)(Int32)item.AMOUNT * (long)(Int32)item.SERVICECOST;
            }
            return res;
        }
        private void DeleteCart(ListViewItem parameter)
        {
            USEDSERVICE serveToDelete = parameter.DataContext as USEDSERVICE;

            var serviceSelectionUC = GetAncestorOfType<ServiceSelectionUC>(parameter);
            foreach (var lvi in FindVisualChildren<ListViewItem>(serviceSelectionUC.ServiceList))
            {
                SERVICE service = lvi.DataContext as SERVICE;
                if (service.SERVICEID == serveToDelete.SERVICEID)
                {
                    CheckBox checkBox = GetVisualChild<CheckBox>(lvi);
                    if (checkBox.IsChecked == true)
                    {
                        checkBox.IsChecked = false;
                    }
                }
            }

            List<USEDSERVICE> temp = new List<USEDSERVICE>();
            ServeList.ForEach(item => temp.Add(item));
            temp.Remove(serveToDelete);
            ServeList = temp;
            TotalPrice = GetTotalPrice(ServeList);
        }

        public void Confirm(ServiceSelectionUC serviceSelectionUC)
        {
            if (CustomMessageBox.Show("Xác nhận đặt tiệc?", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
            {
                MenuUC menuUC = (MenuUC)MainVM.PreviousUCs.Pop();
                SetWeddingInfomationUC setWeddingInfomationUC = (SetWeddingInfomationUC)MainVM.PreviousUCs.Pop();
                WeddingInformationSave(setWeddingInfomationUC);
                InvoiceSave(serviceSelectionUC, menuUC);
                MenuSave(menuUC);
                ServeSave(serviceSelectionUC);
                CustomMessageBox.Show("Đặt tiệc thành công!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
                MainVM.ReturnFirstPage();
            }
        }

        public void WeddingInformationSave(SetWeddingInfomationUC setWeddingInfomationUC)
        {
            newWedding.BALLROOMID = Data.Ins.DB.BALLROOMS.Where(x => x.BALLROOMNAME == setWeddingInfomationUC.hallComboBox.Text).SingleOrDefault().BALLROOMID;
            newWedding.BOOKINGDATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            newWedding.BRIDENAME = setWeddingInfomationUC.txtbride.Text;
            newWedding.GROOMNAME = setWeddingInfomationUC.txtgroom.Text;
            newWedding.TELEPHONE = setWeddingInfomationUC.txtphone.Text;
            newWedding.RESERVEAMOUNT = Convert.ToByte(setWeddingInfomationUC.comboBoxreversedTableAmount.Text);
            newWedding.TABLEAMOUNT = Convert.ToByte(setWeddingInfomationUC.comboBoxTableAmount.Text);
            newWedding.WEDDINGDATE = DateTime.Parse(setWeddingInfomationUC.date.SelectedDate.Value.ToString());
            newWedding.SHIFTID = Data.Ins.DB.SHIFTS.Where(x => x.SHIFTNAME == setWeddingInfomationUC.ShiftComboBox.Text).SingleOrDefault().SHIFTID;
        }

        public void InvoiceSave(ServiceSelectionUC serviceSelectionUC, MenuUC menuUC)
        {
            INVOICE newInvoice = new INVOICE();
            newInvoice.TABLECOST = Convert.ToInt32(menuUC.txtTotalprice.Text.Replace(",", ""));
            newInvoice.WEDDINGCOST = (decimal)newInvoice.TABLECOST * newWedding.TABLEAMOUNT;
            newInvoice.SERVICECOST = Convert.ToInt32(serviceSelectionUC.totalPrice.Text.Replace(",", ""));
            newInvoice.STATUS = 1;
            newInvoice.TOTALCOST = newInvoice.WEDDINGCOST + newInvoice.SERVICECOST;
            newInvoice.REMAININGCOST = newInvoice.TOTALCOST * (decimal)0.9;
            newInvoice.PAYDAY = newWedding.WEDDINGDATE;
            newWedding.DEPOSIT = newInvoice.TOTALCOST * (decimal)0.1;
            Data.Ins.DB.WEDDINGS.Add(newWedding);
            Data.Ins.DB.SaveChanges();
            newInvoice.WEDDINGID = newWedding.WEDDINGID;
            newInvoice.USERNAME = CurrentAccount.Username;
            if (Data.Ins.DB.REPORTDETAILS.Where(x => x.REPORTDATE == DateTime.Now).Count() == 0)
            {
                if (Data.Ins.DB.SALESREPORTS.Where(x => x.REPORTMONTH.Month == DateTime.Now.Month && x.REPORTMONTH.Year == DateTime.Now.Year).Count() == 0)
                {
                    SALESREPORT sALESREPORT = new SALESREPORT();
                    DateTime a = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    sALESREPORT.REPORTMONTH = DateTime.Parse(a.ToShortDateString());
                    sALESREPORT.PAIDWEDDINGAMOUNT = 0;
                    sALESREPORT.BOOKEDWEDDINGAMOUNT = 1;
                    sALESREPORT.PROFIT = newWedding.DEPOSIT;
                    Data.Ins.DB.SALESREPORTS.Add(sALESREPORT);
                }

                REPORTDETAIL rEPORTDETAIL = new REPORTDETAIL();
                rEPORTDETAIL.REPORTDATE = DateTime.Parse(DateTime.Now.ToShortDateString());
                rEPORTDETAIL.BOOKEDWEDDINGAMOUNT = 1;
                rEPORTDETAIL.PAIDWEDDINGAMOUNT = 0;
                rEPORTDETAIL.PROFIT = newWedding.DEPOSIT;
                Data.Ins.DB.REPORTDETAILS.Add(rEPORTDETAIL);

            }
            Data.Ins.DB.INVOICES.Add(newInvoice);
            Data.Ins.DB.SaveChanges();
        }
        public void MenuSave(MenuUC menuUC)
        {
            foreach (DISH item in menuUC.carts.Items)
            {
                MENU menu = new MENU();
                menu.WEDDINGID = newWedding.WEDDINGID;
                menu.DISHID = item.DISHID;
                menu.DISHCOST = item.DISHCOST;
                Data.Ins.DB.MENUS.Add(menu);
                Data.Ins.DB.SaveChanges();
            }
        }
        public void ServeSave(ServiceSelectionUC serviceSelectionUC)
        {
            foreach (USEDSERVICE item in ServeList)
            {
                USEDSERVICE serve = new USEDSERVICE();
                serve.SERVICEID = item.SERVICEID;
                serve.WEDDINGID = newWedding.WEDDINGID;
                serve.SERVICECOST = item.SERVICECOST;
                serve.AMOUNT = item.AMOUNT;
                serve.TOTALCOST = item.SERVICECOST * item.AMOUNT;
                serve.SERVICE = Data.Ins.DB.SERVICES.Where(s => s.SERVICEID == serve.SERVICEID).SingleOrDefault();
                Data.Ins.DB.USEDSERVICES.Add(serve);
                Data.Ins.DB.SaveChanges();
            }
        }

    }
}