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
    internal class EditRoleVM : ViewModelBase
    {
        private List<FUNCTION> functions;
        private List<FUNCTION> selectedFunctions;
        public List<FUNCTION> Functions
        {
            get => functions;
            set
            {
                functions = value;
                OnPropertyChanged("Functions");
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
        public ICommand EditNewRoleCommand { get; set; }
        public EditRoleVM()
        {
            AllCheckedCommand = new RelayCommand<EditRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => AllChecked(parameter));
            CheckedCommand = new RelayCommand<CheckBox>((parameter) => parameter == null ? false : true, (parameter) => Checked(parameter));
            LoadedCommand = new RelayCommand<EditRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => Load(parameter));
            EditNewRoleCommand = new RelayCommand<EditRoleWindow>((parameter) => parameter == null ? false : true, (parameter) => EditNewRole(parameter));
        }
        private void Load(EditRoleWindow parameter)
        {
            Functions = Data.Ins.DB.FUNCTIONS.ToList();
        }
        private void AllChecked(EditRoleWindow parameter)
        {
            bool newVal = (parameter.selectAllCheckBox.IsChecked == true);
            foreach (var item in FindVisualChildren<CheckBox>(parameter.functionList))
            {
                item.IsChecked = newVal;
            }
        }
        private void Checked(CheckBox parameter)
        {
            EditRoleWindow editRoleWindow = GetAncestorOfType<EditRoleWindow>(parameter);
            bool isAllChecked = true;
            foreach (var item in FindVisualChildren<CheckBox>(editRoleWindow.functionList))
            {
                if (item.IsChecked == false)
                {
                    isAllChecked = false;
                    break;
                }
            }
            editRoleWindow.selectAllCheckBox.IsChecked = isAllChecked;

        }
        private void EditNewRole(EditRoleWindow prameter)
        {
            ROLE newRole = new ROLE();

        }
    }
}
