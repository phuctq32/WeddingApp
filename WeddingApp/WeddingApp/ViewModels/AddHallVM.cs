using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Views;
using WeddingApp.Models;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace WeddingApp.ViewModels
{
    internal class AddHallVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand AddHallCommand { get; set; }
        public string TypeName { get; set; }
        private string _name { get => TypeName; set { TypeName = value; OnPropertyChanged("Typename"); } }
        private AddWeddingHallWindow thisWindow;

        public AddHallVM()
        {
            LoadedCommand = new RelayCommand<AddWeddingHallWindow>(parameter => true, parameter => Load(parameter));
            AddHallCommand = new RelayCommand<AddWeddingHallWindow>(parameter => true, parameter => Add(parameter));
        }
        public void Load(AddWeddingHallWindow addWeddingHallWindow)
        {
            List<BALLROOMTYPE> list = Data.Ins.DB.BALLROOMTYPEs.ToList();
            foreach (var item in list)
            {
                addWeddingHallWindow.comboBoxType.Items.Add(item.TYPENAME);
            }
            _name = addWeddingHallWindow.comboBoxType.SelectionBoxItem.ToString();
            thisWindow = addWeddingHallWindow;
            thisWindow.comboBoxType.SelectionChanged += new SelectionChangedEventHandler(SelectionChanged);
        }
        public void SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            string SelectedType = thisWindow.comboBoxType.SelectedItem.ToString();
            thisWindow.txtMinPrice.Text = Data.Ins.DB.BALLROOMTYPEs.Where(x => x.TYPENAME == SelectedType).SingleOrDefault().MINIMUMCOST.ToString();
        }
        public void Add(AddWeddingHallWindow addWeddingHallWindow)
        {
            string HallName = addWeddingHallWindow.txtHallname.Text;
            if(string.IsNullOrEmpty(HallName))
            {
                CustomMessageBox.Show("Vui lòng nhập tên sảnh", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            else if (Data.Ins.DB.BALLROOMs.Where(x=>x.BALLROOMNAME == HallName).Count() >0)
            {
                CustomMessageBox.Show("Sảnh đã tồn tại", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
            }
            else
            {
                if (!Regex.IsMatch(addWeddingHallWindow.txtMaxtable.Text, "^[0-9]"))
                {
                    CustomMessageBox.Show("Vui lòng nhập số lượng bàn tối đa", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                else
                {
                    BALLROOM newBallroom = new BALLROOM();
                    newBallroom.BALLROOMNAME = HallName;
                    newBallroom.MAXIMUMTABLE = Convert.ToInt16(addWeddingHallWindow.txtMaxtable.Text);
                    newBallroom.TYPEID = Data.Ins.DB.BALLROOMTYPEs.Where(x => x.TYPENAME == addWeddingHallWindow.comboBoxType.Text).SingleOrDefault().TYPEID;
                    Data.Ins.DB.BALLROOMs.Add(newBallroom);
                    Data.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Thêm thành công", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
        }
    }
}
