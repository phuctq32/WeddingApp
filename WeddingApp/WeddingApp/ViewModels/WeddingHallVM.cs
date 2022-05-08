using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingApp.Models;
using WeddingApp.Views.UserControls;
using System.Windows.Input;
using System.Windows.Controls;
using WeddingApp.Views;

namespace WeddingApp.ViewModels
{
    internal class WeddingHallVM
    {
        public ICommand LoadedCommand   { get; set; }
        public ICommand AddHallCommand { get; set; }
        public ICommand EditHallCommand { get; set; }
        public ICommand DeleteHallCommand { get; set; }


        public WeddingHallVM()
        {
            LoadedCommand = new RelayCommand<WeddingHallUC>(parameter => true, parameter => Loaded(parameter));
            AddHallCommand = new RelayCommand<WeddingHallUC>(parameter => true, parameter => Add(parameter));
            EditHallCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Edit(parameter));
            DeleteHallCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Delete(parameter));

        }
        public void Loaded(WeddingHallUC weddingHallUC)
        {
            List<BALLROOM> bALLROOMs = new List<BALLROOM>();
            bALLROOMs = Data.Ins.DB.BALLROOMs.ToList();
            weddingHallUC.listBallroom.ItemsSource = bALLROOMs;
        }
        public void Add(WeddingHallUC weddingHallUC)
        {
            AddWeddingHallWindow addWeddingHallWindow = new AddWeddingHallWindow();
            addWeddingHallWindow.ShowDialog();
        }
        public void Edit(ListViewItem listViewItem)
        {
            BALLROOM ballroom = listViewItem.DataContext as BALLROOM;
            EditWeddingHallWindow editWeddingHallWindow = new EditWeddingHallWindow();
            editWeddingHallWindow.txtHallname.Text = ballroom.BALLROOMNAME;
            editWeddingHallWindow.txtMaxTable.Text = ballroom.MAXIMUMTABLE.ToString();
            editWeddingHallWindow.comboBoxHallType.Text = ballroom.TYPEID.ToString();
            editWeddingHallWindow.txtMincost.Text = ballroom.BALLROOMTYPE.MINIMUMCOST.ToString();
            editWeddingHallWindow.ShowDialog();
        }
        public void Delete(ListViewItem listViewItem)
        {
            BALLROOM bALLROOM = listViewItem.DataContext as BALLROOM;
            if(CustomMessageBox.Show("Xóa sảnh cưới?", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
            {
                Data.Ins.DB.BALLROOMs.Remove(bALLROOM);
                Data.Ins.DB.SaveChanges();
                CustomMessageBox.Show("Xóa thành công", System.Windows.MessageBoxButton.OK);
            }

        }
    }
}
