using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;

namespace WeddingApp.ViewModels
{
    internal class EditDishListVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand EditDishCommand { get; set; }
        public DISH dISH;
        public EditDishListVM()
        {
            LoadedCommand = new RelayCommand<EditDishWindow>(parameter => true, parameter => Loaded(parameter));
            EditDishCommand = new RelayCommand<EditDishWindow>(parameter => true, parameter => Edit(parameter));
        }
        public void Loaded(EditDishWindow editDishWindow)
        {
            dISH = Data.Ins.DB.DISHES.Where(x => x.DISHNAME == editDishWindow.txtName1.Text).SingleOrDefault();
        }
        public void Edit(EditDishWindow editProductWindow)
        {
            dISH.DISHNAME = editProductWindow.txtName1.Text;
            if (!Regex.IsMatch(editProductWindow.txtPrice1.Text, @"^[0-9_]+$"))
            {
                editProductWindow.txtPrice1.Focus();
                CustomMessageBox.Show("Giá không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                editProductWindow.txtPrice1.Text = "";
                return;
            }
            dISH.COST = Convert.ToInt32(editProductWindow.txtPrice1.Text);
            dISH.DISHDESCRIPTION = editProductWindow.txtDescription.Text;
            Data.Ins.DB.SaveChanges();
            editProductWindow.Close();
            CustomMessageBox.Show("Sửa thành công món " + editProductWindow.txtName1.Text.ToString());
        }
    }
}
