using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;

namespace WeddingApp.ViewModels
{
    internal class AddRoleVM : ViewModelBase
    {
        private List<FUNCTION> functions;
        private List<FUNCTION> selectedFunctions;
        public List<FUNCTION> Functions
        {
            get => functions;
            set
            {
                functions = value;
                OnPropertyChanged("CurrentFunctions");
            }
        }
        public List<FUNCTION> SelectedFunctions
        {
            get => selectedFunctions;
            set { selectedFunctions = value; OnPropertyChanged("SelectedFunctions"); }
        }
        public ICommand LoadedCommand { get; set; }
        public ICommand AllCheckedCommand { get; set; }
        public ICommand CheckedCommand { get; set; }
        public ICommand AddNewRoleCommand { get; set; }
        public AddRoleVM() 
        {
            AllCheckedCommand = new RelayCommand<AddRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => AllChecked(parameter));
            CheckedCommand = new RelayCommand<CheckBox>((parameter) => parameter == null ? false : true, (parameter) => Checked(parameter));
            LoadedCommand = new RelayCommand<AddRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => Load(parameter));
            AddNewRoleCommand = new RelayCommand<AddRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => AddNewRole(parameter));
        }
        private void Load(AddRoleWindow parameter)
        {
            Functions = Data.Ins.DB.FUNCTIONS.ToList();
        }
        private void AllChecked(AddRoleWindow parameter)
        {
            bool newVal = (parameter.selectAllCheckBox.IsChecked == true);
            foreach (var item in FindVisualChildren<CheckBox>(parameter.functionList))
            {
                item.IsChecked = newVal;
            }
        }
        private void Checked(CheckBox parameter)
        {
            AddRoleWindow addRoleWindow = GetAncestorOfType<AddRoleWindow>(parameter);
            bool isAllChecked = true;
            foreach (var item in FindVisualChildren<CheckBox>(addRoleWindow.functionList))
            {
                if (item.IsChecked == false)
                {
                    isAllChecked = false;
                    break;
                }
            }
            addRoleWindow.selectAllCheckBox.IsChecked = isAllChecked;
            
        }
        private void AddNewRole(AddRoleWindow prameter)
        {
            ROLE newRole = new ROLE();

        }
    }
}
