using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingApp.Models;
using WeddingApp.Views.UserControls;
using System.Windows.Input;

namespace WeddingApp.ViewModels
{
    internal class WeddingListVM
    {
        public ICommand LoadedCommand { get; set; }

        public WeddingListVM()
        {
            LoadedCommand = new RelayCommand<WeddingListUC>(parameter => true, parameter => Load(parameter));
        }
        public void Load(WeddingListUC weddingListUC)
        {
            List<WEDDING> wEDDINGs = new List<WEDDING>();
            wEDDINGs = Data.Ins.DB.WEDDINGs.ToList();
            weddingListUC.weddingList.ItemsSource = wEDDINGs;
        }
    }
}
