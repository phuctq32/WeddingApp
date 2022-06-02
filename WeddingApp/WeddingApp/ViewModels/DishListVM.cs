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
        public ICommand EditProductCommand { get; set; } // nút sửa
        public ICommand DeleteProductCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }
        public DishListVM ()
        {
            LoadedCommand = new RelayCommand<DishListUC>(p => p == null ? false : true, p => Load(p));
            OpenAddDishListWindowCommand = new RelayCommand<DishListUC>((parameter) => { return true; }, (parameter) => OpenAddDishListWindow(parameter));
            EditProductCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Edit(parameter));
            DeleteProductCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Delete(parameter));
            CloseButtonCommand = new RelayCommand<AddDishWindow>((parameter) => true, (parameter) => CloseButton(parameter));
        }
        private void Load(DishListUC p)
        {
            ListDish = Data.Ins.DB.DISHES.ToList();
            p.ListView.ItemsSource = ListDish;
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
            editDishWindow.txtPrice1.Text = editType.COST.ToString();
            editDishWindow.txtName1.Text = editType.DISHNAME;
            editDishWindow.txtDescription.Text = editType.DISHDESCRIPTION;
           // editDishWindow.OutlinedComboBox.Text = editType.DISHTYPE.ToString();
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
