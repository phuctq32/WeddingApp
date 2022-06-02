using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingApp.Views;
using WeddingApp.Models;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace WeddingApp.ViewModels
{
    internal class EditHallVM : ViewModelBase
    {
        public ICommand LoadedCommand   { get; set; }
        public ICommand EditHallCommand { get; set; }
        public BALLROOM editBallroom;
        public EditHallVM()
        {
            LoadedCommand = new RelayCommand<EditWeddingHallWindow>(parameter => true, parameter => Loaded(parameter));
            EditHallCommand = new RelayCommand<EditWeddingHallWindow>(parameter => true, parameter => Edit(parameter));
        }
        public void Loaded(EditWeddingHallWindow editWeddingHallWindow)
        {
            editBallroom = Data.Ins.DB.BALLROOMs.Where(x => x.BALLROOMNAME == editWeddingHallWindow.txtHallname.Text).SingleOrDefault();
        }
        public void Edit(EditWeddingHallWindow editWeddingHallWindow)
        {
            string HallName = editWeddingHallWindow.txtHallname.Text;
            if (Data.Ins.DB.BALLROOMs.Where(x => x.BALLROOMNAME == HallName && x.BALLROOMID != editBallroom.BALLROOMID).Count() > 0)
            {
                CustomMessageBox.Show("Sảnh đã tồn tại", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
            }
            else
            {
                if (!Regex.IsMatch(editWeddingHallWindow.txtMaxTable.Text, "^[0-9]"))
                {
                    CustomMessageBox.Show("Vui lòng nhập số lượng bàn tối đa", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                else
                {
                    editBallroom.BALLROOMNAME = editWeddingHallWindow.txtHallname.Text;
                    editBallroom.MAXIMUMTABLE = Convert.ToInt16(editWeddingHallWindow.txtMaxTable.Text);
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Thay đổi thành công", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
        }
    }
}
