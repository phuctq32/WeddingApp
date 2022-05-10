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
    internal class AddHallTypeVM : ViewModelBase
    {
        public ICommand AddHallTypeCommand { get; set;}
        public AddHallTypeVM()
        {
            AddHallTypeCommand = new RelayCommand<AddWeddingHallTypeWindow>(parameter => true, parameter => AddHallType(parameter));
        }
        public void AddHallType(AddWeddingHallTypeWindow addWeddingHallTypeWindow)
        {
            string HallTypeName;
            int MinPrice;
            if (Check(addWeddingHallTypeWindow))
            {
                HallTypeName = addWeddingHallTypeWindow.txtHallTypeName.Text;
                MinPrice = Convert.ToInt32(addWeddingHallTypeWindow.txtMinPrice.Text);
            }
            else
            {
                return;
            }
            if(Data.Ins.DB.BALLROOMTYPEs.Where(x=>x.TYPENAME == HallTypeName || x.MINIMUMCOST == MinPrice).Count() > 0)
            {
                CustomMessageBox.Show("Loại sảnh đã tồn tại!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            else 
            {
                BALLROOMTYPE newType = new BALLROOMTYPE();
                newType.TYPENAME = HallTypeName;
                newType.MINIMUMCOST = MinPrice;
                Data.Ins.DB.BALLROOMTYPEs.Add(newType);
                Data.Ins.DB.SaveChanges();
            }
        }
        public bool Check(AddWeddingHallTypeWindow addWeddingHallTypeWindow)
        {
            if (string.IsNullOrEmpty(addWeddingHallTypeWindow.txtHallTypeName.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập tên loại sảnh", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                addWeddingHallTypeWindow.txtHallTypeName.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(addWeddingHallTypeWindow.txtMinPrice.Text))
            {
                CustomMessageBox.Show("Vui lòng nhập đơn giá bàn tối thiểu", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                addWeddingHallTypeWindow.txtMinPrice.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
