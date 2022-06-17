using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views;

namespace WeddingApp.ViewModels
{
    internal class EditShiftVM : ViewModelBase
    {
        private SHIFT editingShift;
        public ICommand LoadedCommand { get; set; }
        public ICommand ClickCommand { get; set; }
        public EditShiftVM()
        {
            ClickCommand = new RelayCommand<AddShiftWindow>(parameter => parameter == null ? false : true, (parameter) => Click(parameter));
            LoadedCommand = new RelayCommand<AddShiftWindow>(parameter => parameter == null ? false : true, (parameter) => Load(parameter));
            editingShift = new SHIFT();
        }
        private void Load(AddShiftWindow parameter)
        {
            editingShift = Data.Ins.DB.SHIFTS.Where(shift => shift.SHIFTNAME == parameter.shiftNameTxt.Text.ToString()).SingleOrDefault();
        }

        private void Click(AddShiftWindow parameter)
        {
            if (ShiftListVM.IsEdit)
            {
                Edit(parameter);
            }
            else
            {
                Add(parameter);
            }
        }

        private void Add(AddShiftWindow parameter)
        {
            if (CustomMessageBox.Show("Xác nhận thêm ca?", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
            {
                SHIFT shift = new SHIFT();

                shift.SHIFTNAME = parameter.shiftNameTxt.Text.ToString().Trim();
                TimeSpan startTime, endTime;

                if (!(TimeSpan.TryParse(parameter.shiftStartTime.Text.ToString(), out startTime)))
                {
                    CustomMessageBox.Show("Convert Error!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                shift.STARTTIME = startTime;

                if (!(TimeSpan.TryParse(parameter.shiftEndTime.Text.ToString(), out endTime)))
                {
                    CustomMessageBox.Show("Convert Error!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                shift.ENDTIME = endTime;

                try
                {
                    Data.Ins.DB.SHIFTS.Add(shift);
                    Data.Ins.DB.SaveChanges();
                    parameter.Close();
                }
                catch
                {
                    CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }
        private void Edit(AddShiftWindow parameter)
        {
            if (CustomMessageBox.Show("Lưu thay đổi?", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
            {
                

                try
                {
                    editingShift.SHIFTNAME = parameter.shiftNameTxt.Text.ToString().Trim();
                    TimeSpan startTime, endTime;

                    if (!(TimeSpan.TryParse(parameter.shiftStartTime.Text.ToString(), out startTime)))
                    {
                        CustomMessageBox.Show("Convert Error!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                    editingShift.STARTTIME = startTime;

                    if (!(TimeSpan.TryParse(parameter.shiftEndTime.Text.ToString(), out endTime)))
                    {
                        CustomMessageBox.Show("Convert Error!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                    editingShift.ENDTIME = endTime;

                    Data.Ins.DB.SaveChanges();
                    parameter.Close();
                }
                catch
                {
                    CustomMessageBox.Show("Lỗi cơ sở dữ liệu!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }
    }
}
