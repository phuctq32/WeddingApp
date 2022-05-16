using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Views.UserControls;

namespace WeddingApp.ViewModels
{
    internal class SetWeddingInfoVM : ViewModelBase
    {
        private SetWeddingInfomationUC SetWeddingInfoUserControl { get; set; }
        private MenuUC MenuUserControl { get; set; }
        private ServiceSelectionUC ServiceSelectionUserControl { get; set; } 
        public ICommand SwitchUCCommand { get; set; }
        public SetWeddingInfoVM() 
        {
            SetWeddingInfoUserControl = new SetWeddingInfomationUC();
            MenuUserControl = new MenuUC();
            ServiceSelectionUserControl = new ServiceSelectionUC();
            SwitchUCCommand = new RelayCommand<SetWeddingInfomationUC>(p => true, p => SwitchUC(p));
        }

        private void SwitchUC(SetWeddingInfomationUC parameter)
        {
            
        }
    }
}
