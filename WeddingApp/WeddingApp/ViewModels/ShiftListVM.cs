using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    internal class ShiftListVM : ViewModelBase
    {
        private List<SHIFT> shifts;

        public static bool IsEdit = false;
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
        public ICommand EditSelectedShiftCommand { get; set; }
        public ICommand AddSelectedShiftCommand { get; set; }
        public ShiftListVM()
        {
            LoadedCommand = new RelayCommand<ShiftListUC>(parameter => parameter == null ? false : true, (parameter) => Load(parameter));
            EditSelectedShiftCommand = new RelayCommand<ListViewItem>(parameter => parameter == null ? false : true, (parameter) => EditSelectedShift(parameter));
            AddSelectedShiftCommand = new RelayCommand<ShiftListUC>(parameter => parameter == null ? false : true, (parameter) => AddSelectedShift(parameter));
            
        }

        private void Load(ShiftListUC parameter)
        {
            Shifts = Data.Ins.DB.SHIFTS.ToList();
        }

        private void EditSelectedShift(ListViewItem parameter)
        {
            SHIFT selectedShift = parameter.DataContext as SHIFT;

            
            AddShiftWindow addShiftWindow = new AddShiftWindow();

            addShiftWindow.titleTxt.Text = "Chỉnh sủa ca";
            addShiftWindow.shiftNameTxt.Text = selectedShift.SHIFTNAME.ToString();
            addShiftWindow.shiftStartTime.Text = selectedShift.STARTTIME.ToString(@"hh\:mm");
            addShiftWindow.shiftEndTime.Text = selectedShift.ENDTIME.ToString(@"hh\:mm");
            addShiftWindow.add_save_btn.Content = "Lưu";


            IsEdit = true;
            addShiftWindow.ShowDialog();
            Shifts = Data.Ins.DB.SHIFTS.ToList();

        }

        private void AddSelectedShift(ShiftListUC parameter)
        {
            AddShiftWindow addShiftWindow = new AddShiftWindow();

            IsEdit = false;
            addShiftWindow.ShowDialog();
            Shifts = Data.Ins.DB.SHIFTS.ToList();
        }

        
    }
}
