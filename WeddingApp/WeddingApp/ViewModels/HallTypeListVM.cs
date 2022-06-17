using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingApp.Views.UserControls.Admin;
using WeddingApp.Models;
using WeddingApp.Views;
using System.Windows.Input;
using System.Windows.Controls;

namespace WeddingApp.ViewModels
{
    internal class HallTypeListVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand AddHallTypeCommand { get; set; }
        public ICommand EditHallTypeCommand { get; set; }
        public ICommand DeleteHallTypeCommand { get; set; }

        public HallTypeListVM()
        {
            LoadedCommand = new RelayCommand<WeddingHallTypeListUC>(parameter => true, parameter => Loaded(parameter));
            AddHallTypeCommand = new RelayCommand<WeddingHallTypeListUC>(parameter => true, parameter => Add(parameter));
            EditHallTypeCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Edit(parameter));
            DeleteHallTypeCommand = new RelayCommand<ListViewItem>(parameter => true, parameter => Delete(parameter));
        }
        public void Loaded(WeddingHallTypeListUC weddingHallTypeListUC)
        {
            List<BALLROOMTYPE> listType = Data.Ins.DB.BALLROOMTYPES.ToList();
            weddingHallTypeListUC.ListView.ItemsSource = listType;
        }
        public void Add(WeddingHallTypeListUC weddingHallTypeListUC)
        {
            AddWeddingHallTypeWindow addWeddingHallTypeWindow = new AddWeddingHallTypeWindow();
            addWeddingHallTypeWindow.ShowDialog();
        }
        public void Edit(ListViewItem listViewItem)
        {
            BALLROOMTYPE editType = listViewItem.DataContext as BALLROOMTYPE;
            EditWeddingHallTypeWindow editWeddingHallTypeWindow = new EditWeddingHallTypeWindow();
            editWeddingHallTypeWindow.TypeName.Items.Add(editType.BALLROOMTYPENAME);
            editWeddingHallTypeWindow.TypeName.SelectedIndex = 0;
            editWeddingHallTypeWindow.txtMinPrice.Text = Convert.ToInt32(editType.MINIMUMCOST).ToString();
            editWeddingHallTypeWindow.ShowDialog();
        }
        public void Delete(ListViewItem listViewItem)
        {
            BALLROOMTYPE deleteType = listViewItem.DataContext as BALLROOMTYPE;
            if(CustomMessageBox.Show("Xóa loại sảnh?",System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
            {
                Data.Ins.DB.BALLROOMTYPES.Remove(deleteType);
                try
                {
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Xóa thành công", System.Windows.MessageBoxButton.OK);
                }
                catch
                {
                    CustomMessageBox.Show("Xóa thất bại", System.Windows.MessageBoxButton.OK);
                }
            }
        }
    }
}
