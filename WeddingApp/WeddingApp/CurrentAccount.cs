using WeddingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeddingApp
{
    public class CurrentAccount : ViewModels.ViewModelBase
    {
        private static string username;
        private static string password;
        private static EMPLOYEE employee;
        public CurrentAccount()
        {
        }

        //public static bool IsAdmin { get; set; }
        //public static bool IsUser { get; set; }


        public static string Username { get => username; set => username = value; }
        public static string Password { get => password; set => password = value; }
        public static EMPLOYEE Employee { get => employee; set => employee = value; }
    }
}