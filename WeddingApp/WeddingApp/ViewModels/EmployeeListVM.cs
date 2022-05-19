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
        public ICommand OpenAddEmployeeWindowCommand { get; set; }
       

        public EmployeeListVM()
        {
            OpenAddEmployeeWindowCommand = new RelayCommand<EmployeeListUC>((parameter) => { return true; }, (parameter) => OpenAddEmployee(parameter));
        }

        public void OpenAddEmployee(EmployeeListUC parameter)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.ShowDialog();
        } 
       
    }
}
