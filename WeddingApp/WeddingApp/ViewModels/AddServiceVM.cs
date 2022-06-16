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
    internal class AddServiceVM : ViewModelBase
    {
        public ICommand AddServiceCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }

        public string SelectedImage;
        private string containerName = "service";
        private string connectionString = "DefaultEndpointsProtocol=https;AccountName=imagedish;AccountKey=udbl5BJAHZv8wzmuFf/jE5di0ysn9a8Z8H9ZEBCwUhnFUq8zo0mVqgSdL6Im3rKQeb7uJid2xbA62haXbZ93VA==;EndpointSuffix=core.windows.net";

        private SERVICE service = new SERVICE();

        public AddServiceVM()
        {
            AddServiceCommand = new RelayCommand<AddServiceWindow>((parameter) => true, (parameter) => Add(parameter));
            SelectImageCommand = new RelayCommand<AddServiceWindow>(p => true, p => SelectImage(p));
            CloseButtonCommand = new RelayCommand<AddServiceWindow>((parameter) => true, (parameter) => CloseButton(parameter));
        }

        public void SelectImage(AddServiceWindow addDishWindow)
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

        public void Add(AddServiceWindow parameter)
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

            service.SERVICENAME = parameter.txtName.Text;
            service.SERVICECOST = Convert.ToInt32(parameter.txtPrice.Text);
            service.SERVICEDESCRIPTION = parameter.txtDescription.Text;
            Data.Ins.DB.SERVICES.Add(service);
            Data.Ins.DB.SaveChanges();
            parameter.addServiceWindow.Close();
            if (!string.IsNullOrEmpty(SelectedImage))
            {
                UploadImage();
            }
            CustomMessageBox.Show("Thêm thành công dịch vụ " + parameter.txtName.Text.ToString(), MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                    containerClient.UploadBlob(service.SERVICEID + "." + filename[1], stream);
                }
                catch
                {
                    CustomMessageBox.Show("Cập nhật ảnh không thành công", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }

            //Update new Image link

            //PRODUCT product = Data.Ins.DB.PRODUCTs.Where(x => x.ID_ == Current_Product.ID_).SingleOrDefault();
            service.SERVICEIMAGE = "https://imagedish.blob.core.windows.net/service/" + service.SERVICEID + "." + filename[1];
        }

        public void CloseButton(AddServiceWindow addProductWindow)
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