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
            OpenChangeEmployeeInformationWindowCommand = new RelayCommand<EmployeeListUC>((parameter) => { return true; }, (parameter) => OpenChangeEmployee(parameter));
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
        public void OpenChangeEmployee(EmployeeListUC parameter)
        {
            ChangeEmployeeInformationWindow changeEmployeeWindow = new ChangeEmployeeInformationWindow();
            changeEmployeeWindow.ShowDialog();
        } 
       
    }
}
