using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Views.UserControls;
using WeddingApp.Models;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WeddingApp.ViewModels
{
    internal class SetWeddingInfoVM : ViewModelBase
    {
        public ICommand NextStepCommand { get; set; }
        public ICommand LoadedCommand { get; set;}

        public SetWeddingInfomationUC thisUC;

        public SetWeddingInfoVM()
        {
            NextStepCommand = new RelayCommand<SetWeddingInfomationUC>(p => true, p => NextStep(p));
            LoadedCommand = new RelayCommand<SetWeddingInfomationUC>(p => true, p => Loaded(p));
        }
        public void Loaded(SetWeddingInfomationUC setWeddingInfomationUC)
        {
            List<BALLROOM> listRoom = Data.Ins.DB.BALLROOMs.ToList();
            foreach (var item in listRoom)
            {
                setWeddingInfomationUC.hallComboBox.Items.Add(item.BALLROOMNAME);
            }
            List<SHIFT> listShift = Data.Ins.DB.SHIFTS.ToList();
            foreach(var item in listShift)
            {
                setWeddingInfomationUC.ShiftComboBox.Items.Add(item.SHIFTNAME);
            }
            setWeddingInfomationUC.hallComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(SelectionChanged);
            thisUC = setWeddingInfomationUC;
        }
        public void NextStep(SetWeddingInfomationUC setWeddingInfomationUC)
        {
            if (!Regex.IsMatch(setWeddingInfomationUC.txtphone.Text, @"(^(0)[7-9]|(3)|(5))[0-9]{8}$"))
            {
                CustomMessageBox.Show("Vui lòng nhập đúng số điện thoại", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                setWeddingInfomationUC.txtphone.Focus();
                return;
            }
            if(string.IsNullOrEmpty(setWeddingInfomationUC.txtbride.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập tên chú rể", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                setWeddingInfomationUC.txtbride.Focus();
                return;
            }
            else
            {
                if(!Regex.IsMatch(setWeddingInfomationUC.txtbride.Text, @"([A-vxyỳọáầảấờễàạằệếýộậốũứĩõúữịỗìềểẩớặòùồợãụủíỹắẫựỉỏừỷởóéửỵẳẹèẽổẵẻỡơôưăêâđ]+)((\s{1}[A-vxyỳọáầảấờễàạằệếýộậốũứĩõúữịỗìềểẩớặòùồợãụủíỹắẫựỉỏừỷởóéửỵẳẹèẽổẵẻỡơôưăêâđ]+){1,})"))
                {
                    CustomMessageBox.Show("Vui lòng nhập đúng tên chú rể", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    setWeddingInfomationUC.txtbride.Focus();
                    return;
                }
            }
            if (string.IsNullOrEmpty(setWeddingInfomationUC.txtgroom.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập tên cô dâu", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                setWeddingInfomationUC.txtbride.Focus();
                return;
            }
            else
            {
                if (!Regex.IsMatch(setWeddingInfomationUC.txtgroom.Text, @"([A-vxyỳọáầảấờễàạằệếýộậốũứĩõúữịỗìềểẩớặòùồợãụủíỹắẫựỉỏừỷởóéửỵẳẹèẽổẵẻỡơôưăêâđ]+)((\s{1}[A-vxyỳọáầảấờễàạằệếýộậốũứĩõúữịỗìềểẩớặòùồợãụủíỹắẫựỉỏừỷởóéửỵẳẹèẽổẵẻỡơôưăêâđ]+){1,})"))
                {
                    CustomMessageBox.Show("Vui lòng nhập đúng tên cô dâu", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    setWeddingInfomationUC.txtgroom.Focus();
                    return;
                }
            }
            if (!string.IsNullOrEmpty(setWeddingInfomationUC.date.Text) && !string.IsNullOrEmpty(setWeddingInfomationUC.hallComboBox.Text) && !string.IsNullOrEmpty(setWeddingInfomationUC.ShiftComboBox.Text))
            {
                if (!string.IsNullOrEmpty(setWeddingInfomationUC.comboBoxreversedTableAmount.Text) && !string.IsNullOrEmpty(setWeddingInfomationUC.comboBoxTableAmount.Text))
                {
                    if (setWeddingInfomationUC.date.SelectedDate < DateTime.Now)
                    {
                        CustomMessageBox.Show("Ngày đãi tiệc không hợp lệ", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                    else
                    {
                        DateTime WeddingDate = DateTime.Parse(setWeddingInfomationUC.date.SelectedDate.Value.ToString());
                        List<WEDDING> wedding = Data.Ins.DB.WEDDINGs.Where(x => x.WEDDINGDATE == WeddingDate).ToList();
                        foreach(var item in wedding)
                        {
                            if(item.SHIFT.SHIFTNAME == setWeddingInfomationUC.ShiftComboBox.Text && item.BALLROOM.BALLROOMNAME == setWeddingInfomationUC.hallComboBox.Text)
                            {
                                CustomMessageBox.Show("Sảnh đã được đặt", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                                return;
                            }
                        }
                        MainVM.WeddingHall = setWeddingInfomationUC.hallComboBox.Text;
                        MainVM.NextUC();
                    }
                }
                else
                {
                    CustomMessageBox.Show("Vui lòng nhập thông tin về số lượng bàn tiệc!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.Show("Vui lòng nhập đầy đủ thông tin về ngày tiệc!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        public void SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            string name = thisUC.hallComboBox.SelectedItem.ToString();
            BALLROOM SelectedBallroom = Data.Ins.DB.BALLROOMs.Where(x => x.BALLROOMNAME == name).SingleOrDefault();
            thisUC.comboBoxTableAmount.IsEnabled = true;
            thisUC.comboBoxreversedTableAmount.IsEnabled = true;
            thisUC.comboBoxTableAmount.Items.Clear();
            thisUC.comboBoxreversedTableAmount.Items.Clear();
            for(int i = 1;i<=SelectedBallroom.MAXIMUMTABLE; i++)
            {
                thisUC.comboBoxTableAmount.Items.Add(i);
            }
            for(int i = 1;i<= SelectedBallroom.MAXIMUMTABLE/10;i++)
            {
                thisUC.comboBoxreversedTableAmount.Items.Add(i);
            }
        }
    }
}
