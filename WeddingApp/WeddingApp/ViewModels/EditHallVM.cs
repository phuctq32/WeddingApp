using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingApp.Views;
using WeddingApp.Models;
using System.Windows.Input;

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
            editBallroom.BALLROOMNAME = editWeddingHallWindow.txtHallname.Text;
            editBallroom.MAXIMUMTABLE = Convert.ToInt16(editWeddingHallWindow.txtMaxTable);
            Data.Ins.DB.SaveChanges();
        }
    }
}
