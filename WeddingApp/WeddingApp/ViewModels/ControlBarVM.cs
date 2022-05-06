using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WeddingApp.ViewModels
{
    internal class ControlBarVM : ViewModelBase
    {
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }

        public ControlBarVM()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => p == null ? false : true, p =>
            {
                FrameworkElement window = GetParentWindow(p);
                var w = window as Window;

                if (w != null)
                {
                    w.Close();
                }
            });
            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => p == null ? false : true, p =>
            {
                FrameworkElement window = GetParentWindow(p);
                var w = window as Window;

                if (w != null)
                {
                    w.WindowState = WindowState.Minimized;
                }
            });
            MouseMoveWindowCommand = new RelayCommand<UserControl>((p) => p == null ? false : true, p =>
            {
                FrameworkElement window = GetParentWindow(p);
                var w = window as Window;

                if (w != null)
                {
                    w.DragMove();
                }
            });
        }

        public static FrameworkElement GetParentWindow(UserControl p)
        {
            FrameworkElement res = p;
            while (res.Parent != null)
            {
                res = res.Parent as FrameworkElement;
            }
            return res;
        }
    }
}