using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    class DishListVM : ViewModelBase
    {
        public ICommand OpenAddDishListWindowCommand { get; set; }

        public DishListVM ()
        {
            OpenAddDishListWindowCommand = new RelayCommand<DishListUC>((parameter) => { return true; }, (parameter) => OpenAddDishListWindow(parameter));
        }

        public void OpenAddDishListWindow(DishListUC parameter)
        {
            AddDishWindow addDishListWindow = new AddDishWindow();
            addDishListWindow.ShowDialog();
        }
    }
}
