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
    internal class AddHallVM
    {
        public ICommand LoadedCommand { get; set; }

        public AddHallVM()
        {
            LoadedCommand = new RelayCommand<AddWeddingHallWindow>(parameter => true, parameter => Load(parameter));
        }
        public void Load(AddWeddingHallWindow addWeddingHallWindow)
        {
            List<BALLROOMTYPE> list = Data.Ins.DB.BALLROOMTYPEs.ToList();
            foreach (var item in list)
            {
                addWeddingHallWindow.comboBoxType.Items.Add(item.TYPENAME);
            }

        }
    }
}
