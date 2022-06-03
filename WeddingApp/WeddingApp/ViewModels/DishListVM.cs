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
using System.Windows.Controls.Primitives;

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
        public ICommand EditProductCommand { get; set; } // nút sửa
        public ICommand DeleteProductCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }
        public ICommand ToggleButtonCommand     { get; set; }
        public DishListVM ()
        {
            LoadedCommand = new RelayCommand<DishListUC>(p =>true, p => Load(p));
            OpenAddDishListWindowCommand = new RelayCommand<DishListUC>((parameter) => { return true; }, (parameter) => OpenAddDishListWindow(parameter));
            EditProductCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Edit(parameter));
            DeleteProductCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Delete(parameter));
            CloseButtonCommand = new RelayCommand<AddDishWindow>((parameter) => true, (parameter) => CloseButton(parameter));
            ToggleButtonCommand = new RelayCommand<ListViewItem>(p => true, p => Toggle(p));

        }
        private void Load(DishListUC p)
        {
            ListDish = Data.Ins.DB.DISHES.ToList();
            p.ListView.ItemsSource = ListDish;
            foreach(var item in FindVisualChildren<ToggleButton>(p.ListView))
            {
                var lvi = GetAncestorOfType<ListViewItem>(item);
                DISH thisDish = lvi.DataContext as DISH;
                if(thisDish.STATUS == 0)
                {
                    item.IsChecked = false;
                }
            }
        }
        public void Toggle(ListViewItem listViewItem)
        {
            DISH thisDIsh = listViewItem.DataContext as DISH;
            var tb = GetVisualChild<ToggleButton>(listViewItem);
            if (tb.IsChecked == true)
            {
                thisDIsh.STATUS = 1;
            }
            else
            {
                thisDIsh.STATUS = 0;
            }
            Data.Ins.DB.SaveChanges();
        }
        public void OpenAddDishListWindow(DishListUC parameter)
        {
            AddDishWindow addDishListWindow = new AddDishWindow();
            addDishListWindow.ShowDialog();
        }
        
        public void Edit(ListViewItem listViewItem)
        {
            DISH editType = listViewItem.DataContext as DISH;
            EditDishWindow editDishWindow = new EditDishWindow();
            editDishWindow.txtPrice.Text = Convert.ToInt32(editType.COST).ToString();
            editDishWindow.txtName.Text = editType.DISHNAME;
            editDishWindow.txtDescription.Text = editType.DISHDESCRIPTION;
            List<DISHTYPE> dISHTYPEs = Data.Ins.DB.DISHTYPEs.ToList();
            foreach (var item in dISHTYPEs)
            {
                editDishWindow.OutlinedComboBox.Items.Add(item.TYPENAME);
            }
            editDishWindow.OutlinedComboBox.Text = editType.DISHTYPE.TYPENAME;
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
        public void CloseButton(AddDishWindow addProductWindow)
        {
            //if (!string.IsNullOrEmpty(IMAGE_))
            //{
            //    BlobClient blobClient = new BlobClient(connectionString, containerName, Current_Product.ID_ + "." + Current_Product.IMAGE_.Split('.')[5]);
            //    blobClient.Delete();
            //    IMAGE_ = "";
            //}
            addProductWindow.Close();
        }
    }
}
