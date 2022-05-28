using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingApp.Models;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    internal class RoleListVM : ViewModelBase
    {
        private List<ROLE> roles;
        public List<ROLE> Roles
        {
            get => roles;
            set
            {
                roles = value;
                OnPropertyChanged("Roles");
            }
        }
        public ICommand LoadedCommand { get; set; }
        public RoleListVM()
        {
            LoadedCommand = new RelayCommand<RoleListUC>((parameter) => parameter == null ? false : true, (parameter) => Load(parameter));
        }
        private void Load(RoleListUC parameter)
        {
            Roles = Data.Ins.DB.ROLES.ToList();
        }
    }
}
