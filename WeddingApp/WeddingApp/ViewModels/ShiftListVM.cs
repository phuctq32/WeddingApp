using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    internal class ShiftListVM : ViewModelBase
    {
        private List<SHIFT> shifts;

        public List<SHIFT> Shifts
        {
            get => shifts;
            set
            {
                shifts = value;
                OnPropertyChanged(nameof(Shifts));
            }
        }
        public ICommand LoadedCommand { get; set; }
        public ICommand EditShiftCommand { get; set; }
        public ICommand AddShiftCommand { get; set; }
        public ShiftListVM()
        {
            LoadedCommand = new RelayCommand<ShiftListUC>(parameter => parameter == null ? false : true, (parameter) => Load(parameter));
            EditShiftCommand = new RelayCommand<ListViewItem>(parameter => parameter == null ? false : true, (parameter) => Edit(parameter));
            AddShiftCommand = new RelayCommand<ShiftListUC>(parameter => parameter == null ? false : true, (parameter) => Add(parameter));
        }

        private void Load(ShiftListUC parameter)
        {
            Shifts = Data.Ins.DB.SHIFTS.ToList();
        }

        private void Edit(ListViewItem parameter)
        {

        }

        private void Add(ShiftListUC parameter)
        {

        }
    }
}
