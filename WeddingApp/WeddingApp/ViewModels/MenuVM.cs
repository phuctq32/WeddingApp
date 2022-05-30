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
        public ICommand PreviousUCCommand { get; set; }
        public MenuVM()
        {
            PreviousUCCommand = new RelayCommand<MenuUC>(p => true, p => PreviousUC(p));
        }
        public void PreviousUC(MenuUC menuUC)
        {
            MainVM.PreviousUC();
        }
    }
}
