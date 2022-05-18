using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Views.UserControls;
using WeddingApp.Models;
using System.Windows.Controls;

namespace WeddingApp.ViewModels
{
    internal class SetWeddingInfoVM : ViewModelBase
    {
        public ICommand NextStepCommand { get; set; }
        public ICommand LoadedCommand { get; set;}

        public SetWeddingInfomationUC thisUC;

        public SetWeddingInfoVM()
        {
            NextStepCommand = new RelayCommand<SetWeddingInfomationUC>(p => true, p => MainVM.NextUC());
            LoadedCommand = new RelayCommand<SetWeddingInfomationUC>(p => true, p => Loaded(p));
        }
        public void Loaded(SetWeddingInfomationUC setWeddingInfomationUC)
        {
            List<BALLROOM> listRoom = Data.Ins.DB.BALLROOMs.ToList();
            foreach (var item in listRoom)
            {
                setWeddingInfomationUC.hallComboBox.Items.Add(item.BALLROOMNAME);
            }
            setWeddingInfomationUC.hallComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(SelectionChanged);
            thisUC = setWeddingInfomationUC;
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
