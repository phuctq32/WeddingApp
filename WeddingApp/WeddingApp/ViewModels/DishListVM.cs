using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Model;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;
using System.Windows.Data;

namespace WeddingApp.ViewModels
{
    class DishListVM : ViewModelBase
    {
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
        private string dishName;
        public string DishName
        {
            get => dishName;
            set
            {
                dishName = value;
                OnPropertyChanged("DishName");
            }
        }
        public ICommand OpenAddDishListWindowCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public DishListVM ()
        {
            LoadedCommand = new RelayCommand<DishListUC>(p => p == null ? false : true, p => Load(p));
            OpenAddDishListWindowCommand = new RelayCommand<DishListUC>((parameter) => { return true; }, (parameter) => OpenAddDishListWindow(parameter));
        }
        private void Load(DishListUC p)
        {
            //ListDish = Data
            //ListReceipt = Data.Ins.DB.RECEIPTs.Where(receipt => receipt.STATUS_ == "0").ToList();

        }
        public void OpenAddDishListWindow(DishListUC parameter)
        {
            AddDishWindow addDishListWindow = new AddDishWindow();
            addDishListWindow.ShowDialog();
        }
    }
}
