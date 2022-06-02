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
    internal class AddDishVM : ViewModelBase
    {

        public ICommand AddProductCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public string SelectedImage;

        public AddDishVM()
        {
            LoadedCommand = new RelayCommand<AddDishWindow>(p =>true, p => Loaded(p));
            AddProductCommand = new RelayCommand<AddDishWindow>((parameter) => true, (parameter) => Add(parameter));
            SelectImageCommand = new RelayCommand<AddDishWindow>(p => true, p => SelectImage(p));
        }
        public void Loaded(AddDishWindow addDishWindow)
        {
            List<DISHTYPE> dISHTYPEs = Data.Ins.DB.DISHTYPEs.ToList();
            foreach (var item in dISHTYPEs)
            {
                addDishWindow.OutlinedComboBox.Items.Add(item.TYPENAME);
            }
        }
        public void SelectImage(AddDishWindow addDishWindow)
        {

        }
        public void Add(AddDishWindow parameter)
        {
            // Check Foodname
            if (string.IsNullOrEmpty(parameter.txtName.Text))
            {
                parameter.txtName.Focus();
                CustomMessageBox.Show("Tên món ăn đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //if (!Regex.IsMatch(parameter.txtName.Text, @"^[a-zA-Z0-9_]+$"))
            //{
            //    parameter.txtName.Focus();
            //    return;
            //}

            if (!Regex.IsMatch(parameter.txtPrice.Text, @"^[0-9_]+$"))
            {
                parameter.txtPrice.Focus();
                CustomMessageBox.Show("Giá không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtPrice.Text = "";
                return;
            }
            DISH newProduct = new DISH();
            newProduct.DISHNAME = parameter.txtName.Text;
            newProduct.COST = Convert.ToInt32(parameter.txtPrice.Text);
            //newProduct.TYPEID = parameter.OutlinedComboBox.SelectedIndex;
            //newProduct.DISHID = parameter.OutlinedComboBox.SelectedIndex;
            newProduct.DISHIMAGE = "";
            newProduct.DISHDESCRIPTION = parameter.txtDescription.Text;
            Data.Ins.DB.DISHES.Add(newProduct);
            //Data.Ins.DB.SaveChanges();
            parameter.addDishWindow.Close();

            CustomMessageBox.Show("Thêm thành công món " + parameter.txtName.Text.ToString());
        }
    }
}
