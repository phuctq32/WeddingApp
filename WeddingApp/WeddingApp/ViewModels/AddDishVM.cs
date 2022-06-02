using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WeddingApp.Models;
using WeddingApp.Views;

namespace WeddingApp.ViewModels
{
    internal class AddDishVM : ViewModelBase
    {

        public ICommand AddProductCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand LoadedCommand { get; set; }
        public string SelectedImage;

        public AddDishVM()
        {
            LoadedCommand = new RelayCommand<AddDishWindow>(p =>true, p => Loaded(p));
            AddProductCommand = new RelayCommand<AddDishWindow>((parameter) => true, (parameter) => Add(parameter));
            SelectImageCommand = new RelayCommand<AddDishWindow>(p => true, p => SelectImage(p));
        }
        public void Loaded(AddDishWindow addDishWindow)
        {
            List<DISHTYPE> dISHTYPEs = Data.Ins.DB.DISHTYPEs.ToList();
            foreach (var item in dISHTYPEs)
            {
                addDishWindow.OutlinedComboBox.Items.Add(item.TYPENAME);
            }
        }
        public void SelectImage(AddDishWindow addDishWindow)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedImage = openFileDialog.FileName;
                System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(SelectedImage, UriKind.Absolute);
                bitmap.EndInit();
                addDishWindow.Image.ImageSource = bitmap;
            }
            
        }
        public void Add(AddDishWindow parameter)
        {
            //Check Foodname
            if (string.IsNullOrEmpty(parameter.txtName.Text))
            {
                parameter.txtName.Focus();
                CustomMessageBox.Show("Tên món ăn đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //if (!Regex.IsMatch(parameter.txtName.Text, @"^[a-zA-Z0-9_]+$"))
            //{
            //    parameter.txtName.Focus();
            //    return;
            //}

            if (!Regex.IsMatch(parameter.txtPrice.Text, @"^[0-9_]+$"))
            {
                parameter.txtPrice.Focus();
                CustomMessageBox.Show("Giá không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtPrice.Text = "";
                return;
            }
            DISH newProduct = new DISH();
            newProduct.DISHNAME = parameter.txtName.Text;
            newProduct.COST = Convert.ToInt32(parameter.txtPrice.Text);
            newProduct.TYPEID = Data.Ins.DB.DISHTYPEs.Where(x=>x.TYPENAME == parameter.OutlinedComboBox.Text).SingleOrDefault().TYPEID;
            newProduct.DISHIMAGE = "pack://application:,,,/WeddingApp;component/Resources/Images/" + Path.GetFileName(SelectedImage);
            newProduct.DISHDESCRIPTION = parameter.txtDescription.Text;
            Data.Ins.DB.DISHES.Add(newProduct);
            Data.Ins.DB.SaveChanges();
            parameter.addDishWindow.Close();
            CopyImage();
            CustomMessageBox.Show("Thêm thành công món " + parameter.txtName.Text.ToString(), MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }


        private void CopyImage()
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            // or: Directory.GetCurrentDirectory() gives the same result

            // This will get the current PROJECT bin directory (ie ../bin/)
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            // This will get the current PROJECT directory
            System.IO.File.Copy(SelectedImage, projectDirectory + "\\Resources\\Images\\" + Path.GetFileName(SelectedImage), true);


        }
    }
}
