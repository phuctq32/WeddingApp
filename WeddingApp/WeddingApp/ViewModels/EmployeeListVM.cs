using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    class EmployeeListVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand OpenAddEmployeeWindowCommand { get; set; }
        public ICommand OpenChangeEmployeeInformationWindowCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }

        private List<EMPLOYEE> listEmployee;
        public List<EMPLOYEE> ListEmployee
        {
            get => listEmployee;
            set
            {
                listEmployee = value;
                OnPropertyChanged("ListEmployee");
            }
        }
        public EmployeeListVM()
        {
            LoadedCommand = new RelayCommand<EmployeeListUC>((parameter) => { return true; }, (parameter) => load(parameter));
            OpenAddEmployeeWindowCommand = new RelayCommand<EmployeeListUC>((parameter) => { return true; }, (parameter) => OpenAddEmployee(parameter));
            OpenChangeEmployeeInformationWindowCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => OpenChangeEmployee(parameter));
            DeleteEmployeeCommand = new RelayCommand<ListViewItem>((parameter) => { return true; }, (parameter) => Delete(parameter));

        }
        public void load(EmployeeListUC parameter)
        {
            ListEmployee = Data.Ins.DB.EMPLOYEES.ToList();
            parameter.ListView.ItemsSource = ListEmployee;
        }

        public void OpenAddEmployee(EmployeeListUC parameter)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.ShowDialog();
        } 
        public void OpenChangeEmployee(ListViewItem listViewItem)
        {
            EMPLOYEE changeEmployee = listViewItem.DataContext as EMPLOYEE;
            ChangeEmployeeInformationWindow changeEmployeeWindow = new ChangeEmployeeInformationWindow();
            changeEmployeeWindow.txtEmployeeName.Text = changeEmployee.EMPLOYEENAME;
            changeEmployeeWindow.txtUsername.Text = changeEmployee.USERNAME;
            changeEmployeeWindow.txtSalary.Text = changeEmployee.SALARY.ToString();
            changeEmployeeWindow.txtDate.Text = changeEmployee.STARTWORKING.ToString();
            changeEmployeeWindow.ShowDialog();
        }
        public void Delete(ListViewItem listViewItem)
        {
            EMPLOYEE employee = listViewItem.DataContext as EMPLOYEE;
            if (CustomMessageBox.Show("Xóa nhân viên ?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Data.Ins.DB.EMPLOYEES.Remove(employee);
                Data.Ins.DB.SaveChanges();
                CustomMessageBox.Show("Xóa thành công", System.Windows.MessageBoxButton.OK);
            }
        }


    }
}
