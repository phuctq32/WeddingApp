using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Views;
using WeddingApp.Models;

namespace WeddingApp.ViewModels
{
    internal class EditHallTypeVM:ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand EditHallCommand { get; set; }
        
        public BALLROOMTYPE editBallroomType;
        public EditHallTypeVM()
        {
            LoadedCommand = new RelayCommand<EditWeddingHallTypeWindow>(parameter => true, parameter => Loaded(parameter));
            EditHallCommand = new RelayCommand<EditWeddingHallTypeWindow>(parameter => true, parameter => Edit(parameter));
        }
        public void Loaded(EditWeddingHallTypeWindow editWeddingHallTypeWindow)
        {
            editBallroomType = Data.Ins.DB.BALLROOMTYPES.Where(x => x.BALLROOMTYPENAME == editWeddingHallTypeWindow.TypeName.Text).SingleOrDefault();
        }
        public void Edit(EditWeddingHallTypeWindow editWeddingHallTypeWindow)
        {
            int minimumcost = Convert.ToInt32(editWeddingHallTypeWindow.txtMinPrice.Text);
            if (Data.Ins.DB.BALLROOMTYPES.Where(x => x.MINIMUMCOST == minimumcost).Count() > 0)
            {
                CustomMessageBox.Show("Loại sảnh đã tồn tại", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
            }
            else
            {
                editBallroomType.MINIMUMCOST = Convert.ToInt32(editWeddingHallTypeWindow.txtMinPrice.Text);
                Data.Ins.DB.SaveChanges();
            }
        }
    }
}
