using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using WeddingApp.Models;
using WeddingApp.Views.UserControls;
using WeddingApp.Views;

namespace WeddingApp.ViewModels
{
    internal class MenuVM :ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand PreviousUCCommand { get; set; }
        public ICommand NextUCCommand { get;}

        public MenuVM()
        {
            LoadedCommand = new RelayCommand<MenuUC>(p => true, p => Loaded(p));
            PreviousUCCommand = new RelayCommand<MenuUC>(p => true, p => PreviousUC(p));
            NextUCCommand = new RelayCommand<MenuUC>(p => true, p => NextUC(p));
        }
        public void Loaded(MenuUC menuUC)
        {
            List<DISH> dishList = Data.Ins.DB.DISHES.ToList();
            menuUC.ViewListProducts.ItemsSource = dishList;
        }
        public void PreviousUC(MenuUC menuUC)
        {
            MainVM.PreviousUC();
        }
        public void NextUC(MenuUC menuUC)
        {
            MainVM.NextUC();
        }
    }
}
