using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    internal class DashBoardVM : ViewModelBase
    {

        public ICommand selectCommand { get; set; }
        public ICommand SwitchTabCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand btnExportCommand { get; set; }
        public int TotalProduct { get; set; }
        public int TotalValue { get; set; }
        
        public int TotalReceipt { get; set; }
        public int TotalCustomer { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        DateTime now = DateTime.Now;
        // private List<ReC> receipts;
        int month = 1;
        int day = 1;
        int tmp = 0;
        public DashBoardVM()
        {
            SwitchTabCommand = new RelayCommand<DashBoardUC>(p => true, (p) => SwitchTab(p));
            LoadedCommand = new RelayCommand<DashBoardUC>((parameter) => parameter == null ? false : true, (parameter) => Loaded(parameter));
            btnExportCommand = new RelayCommand<ReportChartUC>((parameter) => parameter == null ? false : true, (parameter) => btnExport(parameter));
            selectCommand = new RelayCommand<ComboBox>((parameter) => true, (parameter) => selectMonth(parameter));
            TotalValue = 234;
            
            TotalReceipt = 200;
            TotalCustomer = 300;
            SeriesCollection = new SeriesCollection

                    {
                        new ColumnSeries
                        {
                            Title = "Doanh Thu",
                            Values = new ChartValues<double> {}
                        }
                    };
        }

        private ReportChartUC monthChartUC = new ReportChartUC();
        private ReportChartUC yearChartUC = new ReportChartUC();
        List<INVOICE> invoiceList;
        int value;
        private void selectMonth(ComboBox item)
        {
           TotalValue = 0;
            for (int i = 0; i < day; i++)
            {
                SeriesCollection[0].Values.RemoveAt(0);
            }
            month = item.SelectedIndex + 1 ;
            day = DateTime.DaysInMonth(2022, month);
            invoiceList = Data.Ins.DB.INVOICES.Where(x => x.PAID.Month == month).ToList();
            value = 0;
            Labels = new string[day];
            for (int i = 1; i <= day; i++)
            {
                Labels[i - 1] = "Ngày " + i.ToString();
                foreach (var invoice in invoiceList)
                {
                    if (invoice.PAID.Day == i) value += Convert.ToInt32(invoice.TOTALCOST);
                }
                /*data = Data.Ins.DB.*/
                SeriesCollection[0].Values.Add(value + 0d);
                TotalValue += value;
                value = 0;
               
            }

        }
        private void SwitchTab(DashBoardUC dashBoardindow)
        {
            int index = dashBoardindow.statusListViewUser.SelectedIndex;
            List<ListViewItem> listViewItems = dashBoardindow.statusListViewUser.Items.Cast<ListViewItem>().ToList();
            ListViewItem listViewItem = listViewItems[index];
            switch (listViewItem.Name)
            {
                case "Ngày":
                    dashBoardindow.selectGrid.Children.Clear();
                    dashBoardindow.selectGrid.Children.Add(new CompletedInvoiceListUC());
                    break;
                case "Tháng":
                    //TotalValue = 0;
                    

                    if (tmp == 1)
                    {
                        for (int i = 0; i < day; i++)
                        {
                            SeriesCollection[0].Values.RemoveAt(0);
                        }
                    }

                    month = now.Month;
                    day = DateTime.DaysInMonth(2022, month);

                    invoiceList = Data.Ins.DB.INVOICES.Where(x => x.PAID.Month == now.Month).ToList();
                    value = 0;

                    Labels = new string[40];
                    for (int i = 1; i <= day; i++)
                    {
                        value = 0;
                        Labels[i - 1] =  "Ngày "+ i.ToString();
                        foreach (var invoice in invoiceList)
                        {
                            if (invoice.PAID.Day == i) value += Convert.ToInt32(invoice.TOTALCOST); 
                    }
                        SeriesCollection[0].Values.Add(value + 0d);
                       TotalValue += value;
                        

                    }

                    monthChartUC.time.Labels = Labels;
                    //adding series will update and animate the chart automatically


                    //also adding values updates and animates the chart automatically


                    Formatter = value => value.ToString();
                    tmp = 1;

                    dashBoardindow.selectGrid.Children.Clear();
                    dashBoardindow.selectGrid.Children.Add(monthChartUC);

                    monthChartUC.monthComboBox.SelectedIndex = now.Month -1;
                    monthChartUC.yearComboBox.Visibility = Visibility.Collapsed;

                    break;

                case "Năm":
                    TotalValue = 0;
                    
                    if (tmp == 1)
                    {
                        for (int i = 0; i < day; i++)
                        {
                            SeriesCollection[0].Values.RemoveAt(0);
                        }
                    }

                    //-----------------------------
                    day = 12;

                    List<INVOICE> invoiceYearList = Data.Ins.DB.INVOICES.Where(x => x.PAID.Year == now.Year).ToList();
                    Labels = new string[40];
                    
                    for (int i = 1; i <= 12; i++)
                    {
                        
                        Labels[i - 1] = "Tháng "+ i.ToString();
                        value = 0;
                        foreach (var invoice in invoiceYearList)
                        {
                            if (invoice.PAID.Month == i) value += Convert.ToInt32(invoice.TOTALCOST);
                        }
                        /*data = Data.Ins.DB.*/
                        SeriesCollection[0].Values.Add(value + 0d);
                        TotalValue += value;
                        
                    }
                    yearChartUC.time.Labels = Labels;
                    //adding series will update and animate the chart automatically


                    //also adding values updates and animates the chart automatically


                    Formatter = value => value.ToString();
                    tmp = 1;
                    //-----------------------------
                    dashBoardindow.selectGrid.Children.Clear();
                    dashBoardindow.selectGrid.Children.Add(yearChartUC);
                    yearChartUC.yearComboBox.SelectedIndex = 0;
                    yearChartUC.monthComboBox.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void Loaded(DashBoardUC parameter)
        { }

        /* {
            TotalProduct = Data.Ins.DB.PRODUCTs.Where(x => x.ACTIVE_ == 1).Count();
            TotalCustomer = Data.Ins.DB.USERS.Count() - 1;
            TotalReceipt = Data.Ins.DB.RECEIPTs.Where(x => x.STATUS_ == "2").Count();
            receipts = Data.Ins.DB.RECEIPTs.ToList();
            TotalValue = calculateTotalSales();
            // set up X axis, display 5 column
            DateTime now = DateTime.Now;
            DateTime _1DayBefore = DateTime.Now.Date.AddDays(-1);
            DateTime _2DayBefore = DateTime.Now.Date.AddDays(-2);
            DateTime _3DayBefore = DateTime.Now.Date.AddDays(-3);
            DateTime _4DayBefore = DateTime.Now.Date.AddDays(-4);

            Labels = new[] {_4DayBefore.ToString("dd.MM"),
                            _3DayBefore.ToString("dd.MM"),
                            _2DayBefore.ToString("dd.MM"),
                            _1DayBefore.ToString("dd.MM"),
                            now.ToString("dd.MM")};
            // calculate sales for each day
            int salesNow = calculateSales(out salesNow, now);
            int sales1DayBefore = calculateSales(out sales1DayBefore, _1DayBefore);
            int sales2DayBefore = calculateSales(out sales2DayBefore, _2DayBefore);
            int sales3DayBefore = calculateSales(out sales3DayBefore, _3DayBefore);
            int sales4DayBefore = calculateSales(out sales4DayBefore, _4DayBefore);
            // create a line and add data into chart
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = now.Year.ToString(),
                    Values = new ChartValues<long> { sales4DayBefore, sales3DayBefore, sales2DayBefore, sales1DayBefore, salesNow },
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 15
                }
            };

            YFormatter = value => value.ToString("N0");
        }*/

        private void btnExport(ReportChartUC parameter)
        {
            CustomMessageBox.Show("Đang xuất file ", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            string filePath = "";
            // tạo SaveFileDialog để lưu file excel
            SaveFileDialog dialog = new SaveFileDialog();

            // chỉ lọc ra các file có định dạng Excel
            dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";

            // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
            }

            // nếu đường dẫn null hoặc rỗng thì báo không hợp lệ và return hàm
            if (string.IsNullOrEmpty(filePath))
            {

                CustomMessageBox.Show("Đường dẫn không hợp lệ", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    // đặt tên người tạo file
                    p.Workbook.Properties.Author = "Sa Đam";

                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = "Báo cáo thống kê";

                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add("Wedding sheet");

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws = p.Workbook.Worksheets[0];

                    // đặt tên cho sheet
                    ws.Name = "Wedding sheet";
                    // fontsize mặc định cho cả sheet
                    ws.Cells.Style.Font.Size = 11;
                    // font family mặc định cho cả sheet
                    ws.Cells.Style.Font.Name = "Calibri";

                    // Tạo danh sách các column header
                    string[] arrColumnHeader = {
                                                "Tháng",
                                                "Doanh thu"
                };

                    // lấy ra số lượng cột cần dùng dựa vào số lượng header
                    var countColHeader = arrColumnHeader.Count();

                    // merge các column lại từ column 1 đến số column header
                    // gán giá trị cho cell vừa merge là Thống kê thông tni User Kteam
                    ws.Cells[1, 1].Value = "Báo cáo doanh thu  Wedding App";
                    ws.Cells[1, 1, 1, countColHeader].Merge = true;
                    // in đậm
                    ws.Cells[1, 1, 1, countColHeader].Style.Font.Bold = true;
                    // căn giữa
                    ws.Cells[1, 1, 1, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int colIndex = 2;
                    int rowIndex = 2;

                    //tạo các header từ column header đã tạo từ bên trên
                    foreach (var item in arrColumnHeader)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];

                        //set màu thành gray
                        var fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        //fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        //căn chỉnh các border
                        var border = cell.Style.Border;
                        border.Bottom.Style =
                            border.Top.Style =
                            border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                        //gán giá trị
                        cell.Value = item;

                        colIndex++;
                    }

                    // lấy ra danh sách UserInfo từ ItemSource của DataGrid
                    /* List<UserInfo> userList = dtgExcel.ItemsSource.Cast<UserInfo>().ToList();

                     // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                     foreach (var item in userList)
                     {
                         // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                         colIndex = 1;

                         // rowIndex tương ứng từng dòng dữ liệu
                         rowIndex++;

                         //gán giá trị cho từng cell
                         ws.Cells[rowIndex, colIndex++].Value = item.Name;

                         // lưu ý phải .ToShortDateString để dữ liệu khi in ra Excel là ngày như ta vẫn thấy.Nếu không sẽ ra tổng số :v
                         ws.Cells[rowIndex, colIndex++].Value = item.Birthday.ToShortDateString();
                     }*/
                    colIndex = 1;
                   
                    rowIndex++;
                     for (int i = 1; i <= 12; i++)
                        {
                        ws.Cells[rowIndex++, colIndex].Value = "a"+i;
                    }
                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                }

                CustomMessageBox.Show("Xuất excel thành công", System.Windows.MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch //(Exception EE)
            {

                CustomMessageBox.Show("Có lỗi khi lưu file!", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}