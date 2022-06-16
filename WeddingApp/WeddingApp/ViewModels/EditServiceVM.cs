using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WeddingApp.Models;
using WeddingApp.Views;
using WeddingApp.Views.UserControls.Admin;

namespace WeddingApp.ViewModels
{
    internal class EditServiceVM : ViewModelBase
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand EditServiceCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }

        public ICommand CloseButtonCommand { get; set; }
        public string SelectedImage;
        private string containerName = "service";
        private string connectionString = "DefaultEndpointsProtocol=https;AccountName=imagedish;AccountKey=udbl5BJAHZv8wzmuFf/jE5di0ysn9a8Z8H9ZEBCwUhnFUq8zo0mVqgSdL6Im3rKQeb7uJid2xbA62haXbZ93VA==;EndpointSuffix=core.windows.net";

        public EditServiceWindow editServiceWD;

        private SERVICE service;

        public EditServiceVM()
        {
            LoadedCommand = new RelayCommand<EditServiceWindow>((parameter) => { return true; }, (parameter) => Load(parameter));
            EditServiceCommand = new RelayCommand<EditServiceWindow>((parameter) => { return true; }, (parameter) => editService(parameter));
            SelectImageCommand = new RelayCommand<EditServiceWindow>(p => true, p => SelectImage(p));
            CloseButtonCommand = new RelayCommand<EditServiceWindow>((parameter) => { return true; }, (parameter) => CloseButton(parameter));
        }

        public void Load(EditServiceWindow parameter)
        {
            editServiceWD = parameter;
            service = Data.Ins.DB.SERVICES.Where(x => x.SERVICENAME == parameter.txtName.Text).SingleOrDefault();
            editServiceWD.txtCost.Text = String.Format("{0:N0}", editServiceWD.txtCost.Text);
        }
        public void SelectImage(EditServiceWindow addDishWindow)
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
        public void editService(EditServiceWindow parameter)
        {
            // Check NAME
            if (string.IsNullOrEmpty(parameter.txtName.Text))
            {
                parameter.txtName.Focus();
                CustomMessageBox.Show("Tên dịch vụ đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!Regex.IsMatch(parameter.txtCost.Text, @"^[0-9_]+$"))
            {
                parameter.txtCost.Focus();
                CustomMessageBox.Show("Giá không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                parameter.txtCost.Text = "";
                return;
            }
            try
            {
                //try to update database
                service.SERVICENAME = parameter.txtName.Text;
                service.SERVICECOST = Convert.ToInt32(parameter.txtCost.Text);
                service.SERVICEDESCRIPTION = parameter.txtDescription.Text;
                UploadImage();
                Data.Ins.DB.SaveChanges();
                CustomMessageBox.Show("Thay đổi thành công!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                CustomMessageBox.Show("Thay đổi không thành công!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            parameter.Close();
        }
        private void UploadImage()
        {
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

            //Update Image
            if (!string.IsNullOrEmpty(service.SERVICEIMAGE))
            {
                BlobClient blobClient = new BlobClient(connectionString, containerName, service.SERVICEID + "." + service.SERVICEIMAGE.Split('.')[5]);
                blobClient.Delete();
            }
            //Get name of Image

            string[] filename = Path.GetFileName(SelectedImage).Split('.');

            //Start upload

            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(SelectedImage)))
            {
                //Upload new Image
                try
                {
                    containerClient.UploadBlob(service.SERVICEID + "." + filename[1], stream);
                    service.SERVICEIMAGE = "https://imagedish.blob.core.windows.net/service/" + service.SERVICEID + "." + filename[1];
                }
                catch
                {
                    CustomMessageBox.Show("Cập nhật ảnh không thành công", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }

            //Update new Image link

            //PRODUCT product = Data.Ins.DB.PRODUCTs.Where(x => x.ID_ == Current_Product.ID_).SingleOrDefault();
            
        }

        public void CloseButton(EditServiceWindow parameter)
        {
            SelectedImage = "";
            parameter.Close();
        }
    }
}