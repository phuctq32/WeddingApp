using Azure.Storage.Blobs;
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
        public ICommand CloseButtonCommand { get; set; }

        public string SelectedImage;
        private string containerName = "container";
        private string connectionString = "DefaultEndpointsProtocol=https;AccountName=imagedish;AccountKey=udbl5BJAHZv8wzmuFf/jE5di0ysn9a8Z8H9ZEBCwUhnFUq8zo0mVqgSdL6Im3rKQeb7uJid2xbA62haXbZ93VA==;EndpointSuffix=core.windows.net";
        private DISH newProduct = new DISH();

        public AddDishVM()
        {
            LoadedCommand = new RelayCommand<AddDishWindow>(p => true, p => Loaded(p));
            AddProductCommand = new RelayCommand<AddDishWindow>((parameter) => true, (parameter) => Add(parameter));
            SelectImageCommand = new RelayCommand<AddDishWindow>(p => true, p => SelectImage(p));
            CloseButtonCommand = new RelayCommand<AddDishWindow>((parameter) => true, (parameter) => CloseButton(parameter));
        }

        public void Loaded(AddDishWindow addDishWindow)
        {
            List<DISHTYPE> dISHTYPEs = Data.Ins.DB.DISHTYPES.ToList();
            foreach (var item in dISHTYPEs)
            {
                addDishWindow.OutlinedComboBox.Items.Add(item.DISHTYPENAME);
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
            if (Data.Ins.DB.DISHES.Where(x => x.DISHNAME == parameter.txtName.Text).Count() > 0)
            {
                CustomMessageBox.Show("Tên món ăn đã tồn tại!", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            newProduct.DISHNAME = parameter.txtName.Text;
            newProduct.DISHCOST = Convert.ToInt32(parameter.txtPrice.Text);
            newProduct.DISHTYPEID = Data.Ins.DB.DISHTYPES.Where(x => x.DISHTYPENAME == parameter.OutlinedComboBox.Text).SingleOrDefault().DISHTYPEID;
            newProduct.DISHDESCRIPTION = parameter.txtDescription.Text;
            Data.Ins.DB.DISHES.Add(newProduct);
            Data.Ins.DB.SaveChanges();
            parameter.addDishWindow.Close();
            if (!string.IsNullOrEmpty(SelectedImage))
            {
                UploadImage();
            }
            CustomMessageBox.Show("Thêm thành công món " + parameter.txtName.Text.ToString(), MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void UploadImage()
        {
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

            //Update Image

            //Get name of Image

            string[] filename = Path.GetFileName(SelectedImage).Split('.');

            //Start upload

            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(SelectedImage)))
            {
                //Upload new Image
                try
                {
                    containerClient.UploadBlob(newProduct.DISHID + "." + filename[1], stream);
                    newProduct.DISHIMAGE = "https://imagedish.blob.core.windows.net/container/" + newProduct.DISHID + "." + filename[1];
                }
                catch
                {
                    CustomMessageBox.Show("Cập nhật ảnh không thành công", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }

            //Update new Image link

            //PRODUCT product = Data.Ins.DB.PRODUCTs.Where(x => x.ID_ == Current_Product.ID_).SingleOrDefault();
        }

        public void CloseButton(AddDishWindow addProductWindow)
        {
            //if (!string.IsNullOrEmpty(IMAGE_))
            //{
            //    BlobClient blobClient = new BlobClient(connectionString, containerName, Current_Product.ID_ + "." + Current_Product.IMAGE_.Split('.')[5]);
            //    blobClient.Delete();
            SelectedImage = "";
            //}
            addProductWindow.Close();
        }
    }
}