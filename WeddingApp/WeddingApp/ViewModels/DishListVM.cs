using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;

namespace WeddingApp.ViewModels
{
    class DishListVM : ViewModelBase
    {

       // private readonly DISH Current_Product;
        private List<DISH> listdish;
        public List<DISH> ListDish
        {
            get => listdish;
            set
            {
                listdish = value;
                OnPropertyChanged("ListDish");
            }
        }
        private string foodName;

        public string FoodName
        { get => foodName; set { foodName = value; OnPropertyChanged(); } }

        private string price;

        public string Price
        { get => price; set { price = value; OnPropertyChanged(); } }

        public ICommand OpenAddDishListWindowCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand EditProductCommand { get; set; } // nút sửa
        public ICommand UpdateCommand { get; set; } // nút cập nhật
        public ICommand DeleteProductCommand { get; set; }

        public DishListVM ()
        {
            LoadedCommand = new RelayCommand<DishListUC>(p => p == null ? false : true, p => Load(p));
            OpenAddDishListWindowCommand = new RelayCommand<DishListUC>((parameter) => { return true; }, (parameter) => OpenAddDishListWindow(parameter));
            AddProductCommand = new RelayCommand<AddDishWindow>((parameter) => true, (parameter) => Add(parameter));
            EditProductCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Edit1(parameter));
            DeleteProductCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Delete(parameter));
            UpdateCommand = new RelayCommand<EditDishWindow>(parameter => true, parameter => Edit2(parameter));
        }
        private void Load(DishListUC p)
        {
            ListDish = Data.Ins.DB.DISHES.ToList();
            p.ListView.ItemsSource = ListDish;
        }
        public void OpenAddDishListWindow(DishListUC parameter)
        {
            AddDishWindow addDishListWindow = new AddDishWindow();
            //listdish = new List<DISH>();
            //List<DISH> a = Data.Ins.DB.DISHES.ToList();
            //int i = 1;
            //foreach (DISH pRODUCT in a)
            //{
            //    if (i < Convert.ToInt32(pRODUCT.DISHID))
            //    {
            //        i = Convert.ToInt32(pRODUCT.DISHID)+1;
            //        listdish.Add(pRODUCT);
            //    }
            //}
            addDishListWindow.ShowDialog();
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

            if (!Regex.IsMatch(parameter.txtName.Text, @"^[a-zA-Z0-9_]+$"))
            {
                parameter.txtName.Focus();
                return;
            }

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
            newProduct.TYPEID = parameter.OutlinedComboBox.SelectedIndex;
            newProduct.DISHID = parameter.OutlinedComboBox.SelectedIndex;
            newProduct.DISHIMAGE = "";
            newProduct.DISHDESCRIPTION = parameter.txtDescription.Text;
            Data.Ins.DB.DISHES.Add(newProduct);
        //  Data.Ins.DB.SaveChanges();
            parameter.addDishWindow.Close();
            
            CustomMessageBox.Show("Thêm thành công món " + parameter.txtName.Text.ToString());
        }
        public void Edit1(ListViewItem listViewItem)
        {

            DISH editType = listViewItem.DataContext as DISH;
            EditDishWindow editDishWindow = new EditDishWindow();
            editDishWindow.txtPrice1.Text = editType.COST.ToString();
            editDishWindow.txtName1.Text = editType.DISHNAME;
            editDishWindow.OutlinedComboBox.Text = editType.DISHTYPE.ToString();
            //editDishWindow.txtDescription.Text = editType.DISHDESCRIPTION.ToString();
            //editDishWindow.recImage = "";
            editDishWindow.ShowDialog();
        }
        public void Delete(ListViewItem listViewItem)
        {
            DISH deleteType = listViewItem.DataContext as DISH;
            if (CustomMessageBox.Show("Xóa món ăn?", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
            {
                Data.Ins.DB.DISHES.Remove(deleteType);
                Data.Ins.DB.SaveChanges();
                CustomMessageBox.Show("Xóa thành công", System.Windows.MessageBoxButton.OK);
            }
        }
        public void Edit2(EditDishWindow editProductWindow)
        {
            DISH editDish = new DISH();
        //if (string.IsNullOrEmpty(editProductWindow.txtName1.Text))
        //{
        //    editProductWindow.txtName1.Focus();
        //    CustomMessageBox.Show("Tên món ăn đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    return;
        //}
            editDish.DISHNAME = editProductWindow.txtName1.Text;
            editDish.COST = Convert.ToInt32(editProductWindow.txtPrice1);
            editDish.DISHDESCRIPTION = editProductWindow.txtDescription.Text;
            Data.Ins.DB.SaveChanges();
            editProductWindow.Close();
            CustomMessageBox.Show("Sửa thành công món " + editProductWindow.txtName1.Text.ToString());
        }
    }
}
