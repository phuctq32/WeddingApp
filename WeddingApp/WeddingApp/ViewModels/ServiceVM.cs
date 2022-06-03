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
        private List<SERVE> serveList;
        private WEDDING newWedding = new WEDDING();

        public ICommand LoadedCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public ICommand AllCheckedCommand { get; set; }
        public ICommand DownCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand ConfirmCommand  { get; set; }
        public ICommand DeleteCartCommand { get; set; }
        
        public long TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }
        public List<SERVE> ServeList
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
            ServeList = new List<SERVE>();
        }
        public void Loaded(ServiceSelectionUC serviceSelectionUC)
        {
            List<SERVICE> service = Data.Ins.DB.SERVICEs.ToList();
            serviceSelectionUC.ServiceList.ItemsSource = service;
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
                cart.COST = cart.AMOUNT * cart.SERVICECOST;
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
                SERVE cart = lvi.DataContext as SERVE;

                //Tăng số lượng của sản phẩm
                amount++;
                cart.AMOUNT = Convert.ToByte(amount);
                cart.COST = cart.AMOUNT * cart.SERVICECOST;
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
            if(newVal)
            {
                List<SERVE> temp = new List<SERVE>();
                foreach (SERVICE item in parameter.ServiceList.Items)
                {
                    SERVE serve = new SERVE();
                    serve.SERVICEID = item.SERVICEID;
                    serve.SERVICECOST = item.COST;
                    serve.AMOUNT = 1;
                    serve.COST = serve.SERVICECOST * serve.AMOUNT;
                    serve.SERVICE = Data.Ins.DB.SERVICEs.Where(s => s.SERVICEID == serve.SERVICEID).SingleOrDefault();
                    temp.Add(serve);
                }
                ServeList = temp;
            }
            else
            {
                List<SERVE> temp = new List<SERVE>();
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
            SERVE serve = new SERVE();
            serve.SERVICEID = selectedService.SERVICEID;
            serve.SERVICECOST = selectedService.COST;
            serve.AMOUNT = 1;
            serve.COST = serve.SERVICECOST * serve.AMOUNT;
            serve.SERVICE = Data.Ins.DB.SERVICEs.Where(s => s.SERVICEID == serve.SERVICEID).SingleOrDefault();
            if (parameter.IsChecked == true)
            {
                List<SERVE> temp = new List<SERVE>();
                ServeList.ForEach(item => temp.Add(item));
                temp.Add(serve);
                ServeList = temp;
            }
            else
            {
                SERVE serveToDelete = ServeList.Find(x => x.SERVICEID == serve.SERVICEID);
                List<SERVE> temp = new List<SERVE>();
                ServeList.ForEach(item => temp.Add(item));
                temp.Remove(serveToDelete);
                ServeList = temp;
            }
            TotalPrice = GetTotalPrice(ServeList);
        }
        private long GetTotalPrice(List<SERVE> serves)
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
            SERVE serveToDelete = parameter.DataContext as SERVE;

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

            List<SERVE> temp = new List<SERVE>();
            ServeList.ForEach(item => temp.Add(item));
            temp.Remove(serveToDelete);
            ServeList = temp;
            TotalPrice = GetTotalPrice(ServeList);
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
                CustomMessageBox.Show("Đặt tiệc thành công!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
            }
        }

        public void WeddingInformationSave(SetWeddingInfomationUC setWeddingInfomationUC)
        {
            newWedding.BALLROOMID = Data.Ins.DB.BALLROOMs.Where(x => x.BALLROOMNAME == setWeddingInfomationUC.hallComboBox.Text).SingleOrDefault().BALLROOMID;
            newWedding.BOOKINGDATE = DateTime.Parse(DateTime.Now.ToShortDateString());
            newWedding.BRIDE = setWeddingInfomationUC.txtbride.Text;
            newWedding.GROOM = setWeddingInfomationUC.txtgroom.Text;
            newWedding.TELEPHONE = setWeddingInfomationUC.txtphone.Text;
            newWedding.RESERVEAMOUNT = Convert.ToByte(setWeddingInfomationUC.comboBoxreversedTableAmount.Text);
            newWedding.TABLEAMOUNT = Convert.ToByte(setWeddingInfomationUC.comboBoxTableAmount.Text);
            newWedding.WEDDINGDATE = DateTime.Parse(setWeddingInfomationUC.date.SelectedDate.Value.ToString());
            newWedding.SHIFTID = Data.Ins.DB.SHIFTS.Where(x => x.SHIFTNAME == setWeddingInfomationUC.ShiftComboBox.Text).SingleOrDefault().SHIFTID;
        }

        public void InvoiceSave(ServiceSelectionUC serviceSelectionUC, MenuUC menuUC)
        {
            INVOICE newInvoice = new INVOICE();
            newInvoice.SERVICECOST = Convert.ToInt32(serviceSelectionUC.totalPrice.Text.Replace(",", ""));
            newInvoice.WEDDINGCOST = Convert.ToInt32(menuUC.txtTotalprice.Text.Replace(",", "")) * newWedding.TABLEAMOUNT + newInvoice.SERVICECOST;
            newInvoice.STATUS = 1;
            newInvoice.TOTALCOST = newInvoice.WEDDINGCOST + newInvoice.SERVICECOST;
            newInvoice.REMAININGCOST = newInvoice.TOTALCOST * (decimal)0.9;
            newWedding.DEPOSIT = newInvoice.TOTALCOST * (decimal)0.1;
            Data.Ins.DB.WEDDINGs.Add(newWedding);
            Data.Ins.DB.SaveChanges();
            newInvoice.WEDDINGID = newWedding.WEDDINGID;
            newInvoice.USERNAME = CurrentAccount.Username;
            if (Data.Ins.DB.REPORTDETAILs.Where(x => x.REPORTDATE == DateTime.Now).Count() == 0)
            {
                if (Data.Ins.DB.SALESREPORTs.Where(x => x.REPORTMONTH.Month == DateTime.Now.Month && x.REPORTMONTH.Year == DateTime.Now.Year).Count() == 0)
                {
                    SALESREPORT sALESREPORT = new SALESREPORT();
                    DateTime a = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    sALESREPORT.REPORTMONTH = a;
                    sALESREPORT.PAIDWEDDING = 0;
                    sALESREPORT.BOOKEDWEDDING = 1;
                    sALESREPORT.PROFIT = newWedding.DEPOSIT;
                    Data.Ins.DB.SALESREPORTs.Add(sALESREPORT);
                }
                else
                {
                    REPORTDETAIL rEPORTDETAIL = new REPORTDETAIL();
                    rEPORTDETAIL.REPORTDATE = DateTime.Now;
                    rEPORTDETAIL.BOOKEDWEDDING = 1;
                    rEPORTDETAIL.PAIDWEDDING = 0;
                    rEPORTDETAIL.PROFIT = newWedding.DEPOSIT;
                    rEPORTDETAIL.RATIO = 1;
                    rEPORTDETAIL.REPORTMONTH = Data.Ins.DB.SALESREPORTs.Where(x => x.REPORTMONTH.Month == DateTime.Now.Month && x.REPORTMONTH.Year == DateTime.Now.Year).SingleOrDefault().REPORTMONTH;
                    Data.Ins.DB.REPORTDETAILs.Add(rEPORTDETAIL);
                }
            }
            else
            {
                REPORTDETAIL rEPORTDETAIL = Data.Ins.DB.REPORTDETAILs.Where(x => x.REPORTDATE == DateTime.Now).SingleOrDefault();
                rEPORTDETAIL.BOOKEDWEDDING += 1;
                rEPORTDETAIL.PROFIT += newWedding.DEPOSIT;
                rEPORTDETAIL.RATIO = rEPORTDETAIL.PAIDWEDDING / (rEPORTDETAIL.PAIDWEDDING + rEPORTDETAIL.BOOKEDWEDDING);
                SALESREPORT sALESREPORT = Data.Ins.DB.SALESREPORTs.Where(x => x.REPORTMONTH == rEPORTDETAIL.REPORTMONTH).SingleOrDefault();
                sALESREPORT.BOOKEDWEDDING += 1;
                sALESREPORT.PROFIT += newWedding.DEPOSIT;
            }
            // ......
            if (Data.Ins.DB.REPORTDETAILs.Where(x => x.REPORTDATE == newWedding.WEDDINGDATE).Count() == 0)
            {
                if (Data.Ins.DB.SALESREPORTs.Where(x => x.REPORTMONTH.Month == newWedding.WEDDINGDATE.Month && x.REPORTMONTH.Year == newWedding.WEDDINGDATE.Year).Count() == 0)
                {
                    SALESREPORT sALESREPORT = new SALESREPORT();
                    DateTime a = new DateTime(newWedding.WEDDINGDATE.Year, newWedding.WEDDINGDATE.Month, 1);
                    sALESREPORT.REPORTMONTH = a;
                    sALESREPORT.PAIDWEDDING = 0;
                    sALESREPORT.BOOKEDWEDDING = 0;
                    sALESREPORT.PROFIT = 0;
                    Data.Ins.DB.SALESREPORTs.Add(sALESREPORT);
                }
                else
                {
                    REPORTDETAIL rEPORTDETAIL = new REPORTDETAIL();
                    rEPORTDETAIL.REPORTDATE = newWedding.WEDDINGDATE;
                    rEPORTDETAIL.BOOKEDWEDDING = 0;
                    rEPORTDETAIL.PAIDWEDDING = 0;
                    rEPORTDETAIL.PROFIT = 0;
                    rEPORTDETAIL.RATIO = 0;
                    rEPORTDETAIL.REPORTMONTH = Data.Ins.DB.SALESREPORTs.Where(x => x.REPORTMONTH.Month == newWedding.WEDDINGDATE.Month && x.REPORTMONTH.Year == newWedding.WEDDINGDATE.Year).SingleOrDefault().REPORTMONTH;
                    Data.Ins.DB.REPORTDETAILs.Add(rEPORTDETAIL);
                }
            }
            newInvoice.PAID = newWedding.WEDDINGDATE;
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
            foreach(SERVE item in ServeList)
            {
                SERVE serve = new SERVE();
                serve.SERVICEID = item.SERVICEID;
                serve.WEDDINGID = newWedding.WEDDINGID;
                serve.SERVICECOST = item.SERVICECOST;
                serve.AMOUNT = item.AMOUNT;
                serve.SERVICE = Data.Ins.DB.SERVICEs.Where(s => s.SERVICEID == serve.SERVICEID).SingleOrDefault();
                Data.Ins.DB.SERVEs.Add(serve);
                Data.Ins.DB.SaveChanges();
            }
        }

    }
}
