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
    internal class EditDishVM
    {
        public ICommand ConfirmCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }

        public string SelectedImage;
        public DISH editDish;
        private string containerName = "container";
        private string connectionString = "DefaultEndpointsProtocol=https;AccountName=imagedish;AccountKey=udbl5BJAHZv8wzmuFf/jE5di0ysn9a8Z8H9ZEBCwUhnFUq8zo0mVqgSdL6Im3rKQeb7uJid2xbA62haXbZ93VA==;EndpointSuffix=core.windows.net";

        public EditDishVM()
        {
            ConfirmCommand = new RelayCommand<EditDishWindow>(p => true, p => Confirm(p));
            SelectImageCommand = new RelayCommand<EditDishWindow>(p => true, p => SelectImage(p));
            CloseButtonCommand = new RelayCommand<EditDishWindow>((parameter) => true, (parameter) => CloseButton(parameter));
        }

        public void Confirm(EditDishWindow editDishWindow)
        {
            editDish = Data.Ins.DB.DISHES.Where(x => x.DISHNAME == editDishWindow.txtName.Text).SingleOrDefault();
            //Check Foodname
            if (string.IsNullOrEmpty(editDishWindow.txtName.Text))
            {
                editDishWindow.txtName.Focus();
                CustomMessageBox.Show("Tên món ăn đang trống!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Data.Ins.DB.DISHES.Where(x => x.DISHNAME == editDishWindow.txtName.Text && x.DISHID != editDish.DISHID).Count() > 0)
            {
                CustomMessageBox.Show("Tên món ăn đã tồn tại!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //if (!Regex.IsMatch(parameter.txtName.Text, @"^[a-zA-Z0-9_]+$"))
            //{
            //    parameter.txtName.Focus();
            //    return;
            //}

            if (!Regex.IsMatch(editDishWindow.txtPrice.Text, @"^[0-9_]+$"))
            {
                editDishWindow.txtPrice.Focus();
                CustomMessageBox.Show("Giá không đúng định dạng!", MessageBoxButton.OK, MessageBoxImage.Warning);
                editDishWindow.txtPrice.Text = "";
                return;
            }

            editDish.DISHNAME = editDishWindow.txtName.Text;
            editDish.DISHCOST = Convert.ToInt32(editDishWindow.txtPrice.Text);
            editDish.DISHTYPEID = Data.Ins.DB.DISHTYPES.Where(x => x.DISHTYPENAME == editDishWindow.OutlinedComboBox.Text).SingleOrDefault().DISHTYPEID;
            editDish.DISHDESCRIPTION = editDishWindow.txtDescription.Text;
            Data.Ins.DB.SaveChanges();
            editDishWindow.Close();
            if (!string.IsNullOrEmpty(SelectedImage))
            {
                UploadImage();
            }
            CustomMessageBox.Show("Thay đổi thành công", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void SelectImage(EditDishWindow editDishWindow)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedImage = openFileDialog.FileName;
                System.Windows.Media.Imaging.BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(SelectedImage, UriKind.Absolute);
                bitmap.EndInit();
                editDishWindow.Image.ImageSource = bitmap;
            }
        }

        private void UploadImage()
        {
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

            //Update Image
            if (!string.IsNullOrEmpty(editDish.DISHIMAGE))
            {
                BlobClient blobClient = new BlobClient(connectionString, containerName, editDish.DISHID + "." + editDish.DISHIMAGE.Split('.')[5]);
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
                    containerClient.UploadBlob(editDish.DISHID + "." + filename[1], stream);
                    editDish.DISHIMAGE = "https://imagedish.blob.core.windows.net/container/" + editDish.DISHID + "." + filename[1];
                }
                catch
                {
                    CustomMessageBox.Show("Cập nhật ảnh không thành công", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }

            //Update new Image link

            //PRODUCT product = Data.Ins.DB.PRODUCTs.Where(x => x.ID_ == Current_Product.ID_).SingleOrDefault();
        }

        public void CloseButton(EditDishWindow editProductWindow)
        {
            //if (!string.IsNullOrEmpty(IMAGE_))
            //{
            //    BlobClient blobClient = new BlobClient(connectionString, containerName, Current_Product.ID_ + "." + Current_Product.IMAGE_.Split('.')[5]);
            //    blobClient.Delete();
            SelectedImage = "";
            editProductWindow.Close();
        }
    }
}